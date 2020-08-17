// <copyright file="LibraryComparison.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    using System.Collections.Generic;
    using System.Linq;
    using Endjin.ApiChange.Api.Diff;
    using Endjin.ApiChange.Api.Query;
    using Mono.Cecil;

    /// <summary>
    /// Compare library versions.
    /// </summary>
    public static class LibraryComparison
    {
        /// <summary>
        /// Reports the changes from one assembly to another.
        /// </summary>
        /// <param name="pathToOldAssembly">The full file path of the old assembly.</param>
        /// <param name="pathToNewAssembly">The full file path of the new assembly.</param>
        /// <returns>
        /// A <see cref="LibraryChanges"/> describing the changes.
        /// </returns>
        public static LibraryChanges DetectChanges(string pathToOldAssembly, string pathToNewAssembly)
        {
            var ad = new AssemblyDiffer(pathToOldAssembly, pathToNewAssembly);

            var qa = new QueryAggregator();
            qa.TypeQueries.Add(new TypeQuery(TypeQueryMode.ApiRelevant));

            qa.MethodQueries.Add(MethodQuery.PublicMethods);
            qa.MethodQueries.Add(MethodQuery.ProtectedMethods);

            qa.FieldQueries.Add(FieldQuery.PublicFields);
            qa.FieldQueries.Add(FieldQuery.ProtectedFields);

            qa.EventQueries.Add(EventQuery.PublicEvents);
            qa.EventQueries.Add(EventQuery.ProtectedEvents);

            AssemblyDiffCollection diffCollection = ad.GenerateTypeDiff(qa);

            var referenceCache = new ReferenceCache();

            var typesAdded = diffCollection
                .AddedRemovedTypes
                .Where(t => t.Operation.IsAdded)
                .Select(t => MakeLibraryType(t.ObjectV1, add: true, referenceCache))
                .ToList();

            var typesChanged = diffCollection
                .ChangedTypes
                .Select(t => MakeLibraryTypeFromDiff(t, referenceCache))
                .ToList();

            var typesRemoved = diffCollection
                .AddedRemovedTypes
                .Where(t => t.Operation.IsRemoved)
                .Select(t => MakeLibraryType(t.ObjectV1, add: false, referenceCache))
                .ToList();

            return new LibraryChanges(typesAdded, typesChanged, typesRemoved);
        }

        private static LibraryType MakeLibraryType(TypeDefinition td, bool add, ReferenceCache referenceCache)
        {
            List<LibraryConstructor> constructors = MakeLibraryConstructorFromDefinitions(td.Methods, referenceCache);
            List<LibraryMethod> methods = MakeLibraryMethodsFromDefinitions(td.Methods, referenceCache);
            List<LibraryProperty> properties = MakeLibraryPropertiesFromDefinitions(td.Properties, referenceCache);
            List<LibraryField> fields = MakeLibraryFieldsFromDefinitions(td.Fields, referenceCache);
            List<LibraryEvent> events = MakeLibraryEventsFromDefinitions(td.Events, referenceCache);

            return new LibraryType(
                td.FullName,
                constructorsAdded: add ? constructors : null,
                constructorsRemoved: !add ? constructors : null,
                methodsAdded: add ? methods : null,
                methodsRemoved: !add ? methods : null,
                propertiesAdded: add ? properties : null,
                propertiesRemoved: !add ? properties : null,
                eventsAdded: add ? events : null,
                eventsRemoved: !add ? events : null,
                fieldsAdded: add ? fields : null,
                fieldsRemoved: !add ? fields : null);
        }

        private static LibraryType MakeLibraryTypeFromDiff(TypeDiff td, ReferenceCache referenceCache)
        {
            return new LibraryType(
                td.TypeV1.FullName,
                constructorsAdded: MakeLibraryConstructorFromDiff(td, referenceCache, added: true),
                constructorsRemoved: MakeLibraryConstructorFromDiff(td, referenceCache, added: false),
                methodsAdded: MakeLibraryMethodsFromDiff(td, referenceCache, added: true),
                methodsRemoved: MakeLibraryMethodsFromDiff(td, referenceCache, added: false),
                propertiesAdded: MakeLibraryPropertiesFromDiff(td, referenceCache, added: true),
                propertiesRemoved: MakeLibraryPropertiesFromDiff(td, referenceCache, added: false),
                eventsAdded: MakeLibraryEventsFromDiff(td, referenceCache, added: true),
                eventsRemoved: MakeLibraryEventsFromDiff(td, referenceCache, added: false),
                fieldsAdded: MakeLibraryFieldsFromDiff(td, referenceCache, added: true),
                fieldsRemoved: MakeLibraryFieldsFromDiff(td, referenceCache, added: false));
        }

        private static List<LibraryConstructor> MakeLibraryConstructorFromDiff(TypeDiff td, ReferenceCache referenceCache, bool added)
        {
            return MakeLibraryConstructorFromDefinitions(FromDiff(td.Methods, added), referenceCache);
        }

        private static List<LibraryConstructor> MakeLibraryConstructorFromDefinitions(IEnumerable<MethodDefinition> methods, ReferenceCache referenceCache)
        {
            return methods
                .Where(md => md.IsConstructor)
                .Select(m => new LibraryConstructor(MakeParameters(m.Parameters, referenceCache)))
                .ToList();
        }

        private static List<LibraryMethod> MakeLibraryMethodsFromDiff(TypeDiff td, ReferenceCache referenceCache, bool added)
        {
            return MakeLibraryMethodsFromDefinitions(FromDiff(td.Methods, added), referenceCache);
        }

        private static List<LibraryMethod> MakeLibraryMethodsFromDefinitions(IEnumerable<MethodDefinition> methods, ReferenceCache referenceCache)
        {
            return methods
                .Where(md => !md.IsSpecialName)
                .Select(d => new LibraryMethod(
                    d.Name,
                    referenceCache.GetTypeReference(d.ReturnType),
                    MakeParameters(d.Parameters, referenceCache)))
                .ToList();
        }

        private static List<LibraryProperty> MakeLibraryPropertiesFromDiff(TypeDiff td, ReferenceCache referenceCache, bool added)
        {
            // Frustratingly, ApiChange.Api's TypeDiff doesn't have a Properties property, so we
            // have to infer what properties were added or removed by looking in Methods. (It does
            // this work for us with Events, which, like Properties, are methods plus some
            // metadata, but inexplicably, the diff doesn't appear to report Properties in the same
            // way.)
            TypeDefinition source = added ? td.TypeV2 : td.TypeV1;
            IEnumerable<PropertyDefinition> properties = td
                .Methods
                .Where(md => (md.ObjectV1.IsGetter || md.ObjectV1.IsSetter) &&
                    (added ? md.Operation.IsAdded : md.Operation.IsRemoved))
                .GroupBy(md => source.Properties.Single(p => p.GetMethod == md.ObjectV1 || p.SetMethod == md.ObjectV1))
                .Select(g => g.Key);

            return MakeLibraryPropertiesFromDefinitions(properties, referenceCache);
        }

        private static List<LibraryProperty> MakeLibraryPropertiesFromDefinitions(IEnumerable<PropertyDefinition> properties, ReferenceCache referenceCache)
        {
            return properties
                .Select(d => new LibraryProperty(
                    d.Name,
                    referenceCache.GetTypeReference(d.PropertyType)))
                .ToList();
        }

        private static List<LibraryEvent> MakeLibraryEventsFromDiff(TypeDiff td, ReferenceCache referenceCache, bool added)
        {
            return MakeLibraryEventsFromDefinitions(FromDiff(td.Events, added), referenceCache);
        }

        private static List<LibraryEvent> MakeLibraryEventsFromDefinitions(IEnumerable<EventDefinition> events, ReferenceCache referenceCache)
        {
            return events
                .Select(d => new LibraryEvent(
                    d.Name,
                    referenceCache.GetTypeReference(d.EventType)))
                .ToList();
        }

        private static List<LibraryField> MakeLibraryFieldsFromDiff(TypeDiff td, ReferenceCache referenceCache, bool added)
        {
            return MakeLibraryFieldsFromDefinitions(FromDiff(td.Fields, added), referenceCache);
        }

        private static List<LibraryField> MakeLibraryFieldsFromDefinitions(IEnumerable<FieldDefinition> fields, ReferenceCache referenceCache)
        {
            // The type comparer seems to report private fields if they are compiler-generated ones
            // associated with non-private events or properties. This seems surprising, and may be
            // a bug in the comparison library we're using. We don't want to include private fields
            // and we told the QueryAggregator that, but we seem to get them anyway, hence the
            // filter here.
            return fields
                .Where(fd => !fd.IsPrivate)
                .Select(d => new LibraryField(
                    d.Name,
                    referenceCache.GetTypeReference(d.FieldType)))
                .ToList();
        }

        private static List<LibraryParameter> MakeParameters(IEnumerable<ParameterDefinition> parameterDefinitions, ReferenceCache referenceCache)
        {
            return parameterDefinitions
                .Select(pd => new LibraryParameter(pd.Name, referenceCache.GetTypeReference(pd.ParameterType)))
                .ToList();
        }

        private static IEnumerable<T> FromDiff<T>(DiffCollection<T> source, bool added) => source.
            Where(dr => added ? dr.Operation.IsAdded : dr.Operation.IsRemoved)
            .Select(dr => dr.ObjectV1);

        private class ReferenceCache
        {
            private readonly Dictionary<MetadataToken, LibraryAssemblyReference> assemblyRefs = new Dictionary<MetadataToken, LibraryAssemblyReference>();
            private readonly Dictionary<(LibraryAssemblyReference lib, string fullName), LibraryTypeReference> typeRefs = new Dictionary<(LibraryAssemblyReference lib, string fullName), LibraryTypeReference>();

            public LibraryTypeReference GetTypeReference(TypeReference typeReference)
            {
                LibraryAssemblyReference assemblyReference = this.GetAssemblyReference((AssemblyNameReference)typeReference.Scope);
                (LibraryAssemblyReference lib, string fullName) key = (assemblyReference, typeReference.FullName);

                if (!this.typeRefs.TryGetValue(key, out LibraryTypeReference result))
                {
                    result = assemblyReference.ShortName == "mscorlib" && typeReference.FullName == "System.Void"
                        ? null
                        : new LibraryTypeReference(assemblyReference, typeReference.FullName);

                    this.typeRefs.Add(key, result);
                }

                return result;
            }

            private LibraryAssemblyReference GetAssemblyReference(AssemblyNameReference assemblyNameRef)
            {
                if (!this.assemblyRefs.TryGetValue(assemblyNameRef.MetadataToken, out LibraryAssemblyReference result))
                {
                    result = new LibraryAssemblyReference(assemblyNameRef.Name, assemblyNameRef.FullName);
                    this.assemblyRefs.Add(assemblyNameRef.MetadataToken, result);
                }

                return result;
            }
        }
    }
}
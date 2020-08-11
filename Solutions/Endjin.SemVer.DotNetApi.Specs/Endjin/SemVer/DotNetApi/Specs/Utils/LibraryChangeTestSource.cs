// <copyright file="LibraryChangeTestSource.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Specs.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Endjin.SemVer.DotNetApi.Versioning;

    /// <summary>
    /// Generates test combinations for library changes.
    /// </summary>
    public class LibraryChangeTestSource
    {
        private static readonly IEnumerable<ChangeTypes> DefaultPermutations = new[] { ChangeTypes.None };

        private ChangeTypes requiredChanges;
        private ChangeTypes permutationChanges;
        private IEnumerable<ChangeTypes> permutationsEnumerable = DefaultPermutations;

        public LibraryChangeTestSource()
        {
            this.Require = new RequiredChanges(this);
            this.Permutations = new ChangePermutations(this);
        }

        /// <summary>
        /// Enables changes in a public library to be described.
        /// </summary>
        public interface IChanges
        {
            /// <summary>
            /// Add a new public method to an existing type in the library.
            /// </summary>
            void AddPublicField();

            /// <summary>
            /// Add a new public method to an existing type in the library.
            /// </summary>
            void AddPublicConstructor();

            /// <summary>
            /// Add a new public method to an existing type in the library.
            /// </summary>
            void AddPublicMethod();

            /// <summary>
            /// Add a new public property to an existing type in the library.
            /// </summary>
            void AddPublicProperty();

            /// <summary>
            /// Add a new public type to the library.
            /// </summary>
            void AddPublicType();

            /// <summary>
            /// Remove an existing public method from an existing type in the library.
            /// </summary>
            void RemovePublicField();

            /// <summary>
            /// Remove an existing public constructor from an existing type in the library.
            /// </summary>
            void RemovePublicConstructor();

            /// <summary>
            /// Remove an existing public method from an existing type in the library.
            /// </summary>
            void RemovePublicMethod();

            /// <summary>
            /// Remove an existing public property from an existing type in the library.
            /// </summary>
            void RemovePublicProperty();

            /// <summary>
            /// Remove an existing public type from the library.
            /// </summary>
            void RemovePublicType();
        }

        /// <summary>
        /// Gets an <see cref="IChanges"/> that controls the characteristics of all the change
        /// variations produced by this source.
        /// </summary>
        public IChanges Require { get; }

        /// <summary>
        /// Gets an <see cref="IChanges"/> that determines the variations produced by this source.
        /// </summary>
        public IChanges Permutations { get; }

        /// <summary>
        /// Get a sequence <see cref="LibraryChanges"/> with the characteristics and variations
        /// specified through <see cref="Require"/> and <see cref="Permutations"/>.
        /// </summary>
        /// <returns>
        /// A sequence of <see cref="(ChangeTypes Type, LibraryChanges Changes)"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This will always produce at least one <see cref="LibraryChanges"/> which has only those
        /// changes specified to <see cref="Require"/>. If you have specified any
        /// <see cref="Permutations"/>, this will also return one item for each combination of
        /// characteristics specified to <see cref="Permutations"/> (each of which will include
        /// the changes specified to <see cref="Require"/>).
        /// </para>
        /// <para>
        /// For example, if you tell this instance that you require the addition of a public
        /// type, and also that you want permutations in which a public method is added, and in
        /// which a public property is added, <see cref="GetChanges"/> will return four
        /// <see cref="LibraryChanges"/>. All four will add a new public type, and the first will
        /// feature only that change. The others will additionally add a public method, or a public
        /// property, or both.
        /// </para>
        /// </remarks>
        public IEnumerable<(ChangeTypes Type, LibraryChanges Changes)> GetChanges()
        {
            return this.permutationsEnumerable
                .Select(changeType => changeType | this.requiredChanges)
                .Select(changeType => (Type: changeType, Changes: MakeChanges(changeType)));
        }

        private static LibraryChanges MakeChanges(ChangeTypes changeTypes)
        {
            var typesAdded = new List<LibraryType>();
            var typesChanged = new List<LibraryType>();
            var typesRemoved = new List<LibraryType>();

            var mscorlibRef = new LibraryAssemblyReference("mscorlib", "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            var intTypeRef = new LibraryTypeReference(mscorlibRef, "System.Int32");

            int typeNumber = 1;
            if (changeTypes.Includes(ChangeTypes.AddPublicField))
            {
                var typeWithNewField = new LibraryType($"ChangedTypes.WithNewField{typeNumber++}", fieldsAdded: new[] { new LibraryField("NewField", intTypeRef) });
                typesChanged.Add(typeWithNewField);
            }

            if (changeTypes.Includes(ChangeTypes.AddPublicConstructor))
            {
                var typeWithNewMethod = new LibraryType($"ChangedTypes.WithNewConstructor{typeNumber++}", constructorsAdded: new[] { new LibraryConstructor() });
                typesChanged.Add(typeWithNewMethod);
            }

            if (changeTypes.Includes(ChangeTypes.AddPublicMethod))
            {
                var typeWithNewMethod = new LibraryType($"ChangedTypes.WithNewMethod{typeNumber++}", methodsAdded: new[] { new LibraryMethod("NewMethod") });
                typesChanged.Add(typeWithNewMethod);
            }

            if (changeTypes.Includes(ChangeTypes.AddPublicProperty))
            {
                var typeWithNewProperty = new LibraryType($"ChangedTypes.WithNewProperty{typeNumber++}", propertiesAdded: new[] { new LibraryProperty("NewProperty", intTypeRef) });
                typesChanged.Add(typeWithNewProperty);
            }

            if (changeTypes.Includes(ChangeTypes.AddPublicType))
            {
                var newType = new LibraryType($"NewTypes.Type{typeNumber++}", propertiesAdded: new[] { new LibraryProperty("NewProperty", intTypeRef) }, methodsAdded: new[] { new LibraryMethod("NewMethod") });
                typesAdded.Add(newType);
            }

            if (changeTypes.Includes(ChangeTypes.RemovePublicField))
            {
                var typeWithFieldRemoved = new LibraryType($"ChangedTypes.WithFieldRemoved{typeNumber++}", fieldsRemoved: new[] { new LibraryField("OldField", intTypeRef) });
                typesChanged.Add(typeWithFieldRemoved);
            }

            if (changeTypes.Includes(ChangeTypes.RemovePublicConstructor))
            {
                var typeWithFieldRemoved = new LibraryType($"ChangedTypes.WithConstructorRemoved{typeNumber++}", constructorsRemoved: new[] { new LibraryConstructor() });
                typesChanged.Add(typeWithFieldRemoved);
            }

            if (changeTypes.Includes(ChangeTypes.RemovePublicMethod))
            {
                var typeWithMethodRemoved = new LibraryType($"ChangedTypes.WithMethodRemoved{typeNumber++}", methodsRemoved: new[] { new LibraryMethod("OldMethod") });
                typesChanged.Add(typeWithMethodRemoved);
            }

            if (changeTypes.Includes(ChangeTypes.RemovePublicProperty))
            {
                var typeWithPropertyRemoved = new LibraryType($"ChangedTypes.WithPropertyRemoved{typeNumber++}", propertiesRemoved: new[] { new LibraryProperty("OldProperty", intTypeRef) });
                typesChanged.Add(typeWithPropertyRemoved);
            }

            if (changeTypes.Includes(ChangeTypes.RemovePublicType))
            {
                var oldType = new LibraryType($"OldTypes.Type{typeNumber++}", propertiesRemoved: new[] { new LibraryProperty("OldProperty", intTypeRef) }, methodsRemoved: new[] { new LibraryMethod("OldMethod") });
                typesRemoved.Add(oldType);
            }

            return new LibraryChanges(typesAdded, typesChanged, typesRemoved);
        }

        private abstract class ChangesBase : IChanges
        {
            protected ChangesBase(LibraryChangeTestSource parent)
            {
                this.Parent = parent;
            }

            protected LibraryChangeTestSource Parent { get; }

            void IChanges.AddPublicField() => this.AddChange(ChangeTypes.AddPublicField);

            void IChanges.AddPublicConstructor() => this.AddChange(ChangeTypes.AddPublicConstructor);

            void IChanges.AddPublicMethod() => this.AddChange(ChangeTypes.AddPublicMethod);

            void IChanges.AddPublicProperty() => this.AddChange(ChangeTypes.AddPublicProperty);

            void IChanges.AddPublicType() => this.AddChange(ChangeTypes.AddPublicType);

            void IChanges.RemovePublicField() => this.AddChange(ChangeTypes.RemovePublicField);

            void IChanges.RemovePublicConstructor() => this.AddChange(ChangeTypes.RemovePublicConstructor);

            void IChanges.RemovePublicMethod() => this.AddChange(ChangeTypes.RemovePublicMethod);

            void IChanges.RemovePublicProperty() => this.AddChange(ChangeTypes.RemovePublicProperty);

            void IChanges.RemovePublicType() => this.AddChange(ChangeTypes.RemovePublicType);

            protected abstract void AddChange(ChangeTypes changeType);
        }

        private class RequiredChanges : ChangesBase
        {
            internal RequiredChanges(LibraryChangeTestSource parent)
                : base(parent)
            {
            }

            protected override void AddChange(ChangeTypes changeType)
            {
                if (this.Parent.requiredChanges.Includes(changeType))
                {
                    throw new InvalidOperationException($"A required change type of {changeType} has already been specified");
                }

                this.Parent.requiredChanges |= changeType;
            }
        }

        private class ChangePermutations : ChangesBase
        {
            public ChangePermutations(LibraryChangeTestSource parent)
                : base(parent)
            {
            }

            protected override void AddChange(ChangeTypes changeType)
            {
                if (this.Parent.requiredChanges.Includes(changeType))
                {
                    throw new InvalidOperationException($"A required change type of {changeType} has already been specified, so you cannot add it as a permutation");
                }

                if (this.Parent.permutationChanges.Includes(changeType))
                {
                    throw new InvalidOperationException($"A permutation type of {changeType} has already been specified");
                }

                this.Parent.permutationChanges |= changeType;

                this.Parent.permutationsEnumerable = this.Parent.permutationsEnumerable.SelectMany(t => new[] { t, t | changeType });
            }
        }
    }
}

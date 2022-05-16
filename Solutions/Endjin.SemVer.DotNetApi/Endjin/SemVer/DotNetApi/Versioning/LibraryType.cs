// <copyright file="LibraryType.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes a type which has added, removed, or changed in some way.
    /// </summary>
    public class LibraryType
    {
        private static readonly LibraryConstructor[] EmptyConstructors = { };
        private static readonly LibraryMethod[] EmptyMethods = { };
        private static readonly LibraryProperty[] EmptyProperties = { };
        private static readonly LibraryEvent[] EmptyEvents = { };
        private static readonly LibraryField[] EmptyFields = { };

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryType"/> class.
        /// </summary>
        /// <param name="fullName">The <see cref="FullName"/>.</param>
        /// <param name="constructorsAdded">The <see cref="ConstructorsAdded"/>.</param>
        /// <param name="constructorsRemoved">The <see cref="ConstructorsRemoved"/>.</param>
        /// <param name="methodsAdded">The <see cref="MethodsAdded"/>.</param>
        /// <param name="methodsRemoved">The <see cref="MethodsRemoved"/>.</param>
        /// <param name="propertiesAdded">The <see cref="PropertiesAdded"/>.</param>
        /// <param name="propertiesRemoved">The <see cref="PropertiesRemoved"/>.</param>
        /// <param name="eventsAdded">The <see cref="EventsAdded"/>.</param>
        /// <param name="eventsRemoved">The <see cref="EventsRemoved"/>.</param>
        /// <param name="fieldsAdded">The <see cref="FieldsAdded"/>.</param>
        /// <param name="fieldsRemoved">The <see cref="FieldsRemoved"/>.</param>
        public LibraryType(
            string fullName,
            IReadOnlyList<LibraryConstructor> constructorsAdded = null,
            IReadOnlyList<LibraryConstructor> constructorsRemoved = null,
            IReadOnlyList<LibraryMethod> methodsAdded = null,
            IReadOnlyList<LibraryMethod> methodsRemoved = null,
            IReadOnlyList<LibraryProperty> propertiesAdded = null,
            IReadOnlyList<LibraryProperty> propertiesRemoved = null,
            IReadOnlyList<LibraryEvent> eventsAdded = null,
            IReadOnlyList<LibraryEvent> eventsRemoved = null,
            IReadOnlyList<LibraryField> fieldsAdded = null,
            IReadOnlyList<LibraryField> fieldsRemoved = null)
        {
            this.FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            this.ConstructorsAdded = constructorsAdded ?? EmptyConstructors;
            this.ConstructorsRemoved = constructorsRemoved ?? EmptyConstructors;
            this.MethodsAdded = methodsAdded ?? EmptyMethods;
            this.MethodsRemoved = methodsRemoved ?? EmptyMethods;
            this.PropertiesAdded = propertiesAdded ?? EmptyProperties;
            this.PropertiesRemoved = propertiesRemoved ?? EmptyProperties;
            this.EventsAdded = eventsAdded ?? EmptyEvents;
            this.EventsRemoved = eventsRemoved ?? EmptyEvents;
            this.FieldsAdded = fieldsAdded ?? EmptyFields;
            this.FieldsRemoved = fieldsRemoved ?? EmptyFields;
        }

        /// <summary>
        /// Gets the fully-qualified name of this type.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets a list of constructors added to this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is an addition, this will list all of its constructors.
        /// </remarks>
        public IReadOnlyList<LibraryConstructor> ConstructorsAdded { get; }

        /// <summary>
        /// Gets a list of constructors removed from this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is being removed, this will list all of its constructors.
        /// </remarks>
        public IReadOnlyList<LibraryConstructor> ConstructorsRemoved { get; }

        /// <summary>
        /// Gets a list of methods added to this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is an addition, this will list all of its methods.
        /// </remarks>
        public IReadOnlyList<LibraryMethod> MethodsAdded { get; }

        /// <summary>
        /// Gets a list of methods removed from this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is being removed, this will list all of its methods.
        /// </remarks>
        public IReadOnlyList<LibraryMethod> MethodsRemoved { get; }

        /// <summary>
        /// Gets a list of properties added to this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is an addition, this will list all of its properties.
        /// </remarks>
        public IReadOnlyList<LibraryProperty> PropertiesAdded { get; }

        /// <summary>
        /// Gets a list of properties removed from this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is being removed, this will list all of its properties.
        /// </remarks>
        public IReadOnlyList<LibraryProperty> PropertiesRemoved { get; }

        /// <summary>
        /// Gets a list of events added to this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is an addition, this will list all of its events.
        /// </remarks>
        public IReadOnlyList<LibraryEvent> EventsAdded { get; }

        /// <summary>
        /// Gets a list of events removed from this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is being removed, this will list all of its events.
        /// </remarks>
        public IReadOnlyList<LibraryEvent> EventsRemoved { get; }

        /// <summary>
        /// Gets a list of fields added to this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is an addition, this will list all of its fields.
        /// </remarks>
        public IReadOnlyList<LibraryField> FieldsAdded { get; }

        /// <summary>
        /// Gets a list of fields removed from this type.
        /// </summary>
        /// <remarks>
        /// If the type itself is being removed, this will list all of its fields.
        /// </remarks>
        public IReadOnlyList<LibraryField> FieldsRemoved { get; }

        /// <inheritdoc />
        public override string ToString() => this.FullName;
    }
}

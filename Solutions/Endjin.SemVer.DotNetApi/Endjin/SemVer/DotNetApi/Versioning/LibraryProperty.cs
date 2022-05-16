// <copyright file="LibraryProperty.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    /// <summary>
    /// Describes a property which has added, removed, or changed in some way.
    /// </summary>
    public class LibraryProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryProperty"/> class.
        /// </summary>
        /// <param name="name">The property's <see cref="Name"/>.</param>
        /// <param name="type">The property's <see cref="Type"/>.</param>
        public LibraryProperty(string name, LibraryTypeReference type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets the property's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the property's type.
        /// </summary>
        public LibraryTypeReference Type { get; }
    }
}
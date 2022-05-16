// <copyright file="LibraryField.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    /// <summary>
    /// Describes a field which has added or removed.
    /// </summary>
    public class LibraryField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryField"/> class.
        /// </summary>
        /// <param name="name">The field's <see cref="Name"/>.</param>
        /// <param name="type">The field's <see cref="Type"/>.</param>
        public LibraryField(string name, LibraryTypeReference type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets the field's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the field's type.
        /// </summary>
        public LibraryTypeReference Type { get; }
    }
}
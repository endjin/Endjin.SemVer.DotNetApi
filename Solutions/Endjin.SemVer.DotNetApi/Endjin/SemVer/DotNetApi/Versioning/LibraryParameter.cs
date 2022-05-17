// <copyright file="LibraryParameter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    /// <summary>
    /// Describes a parameter of a method.
    /// </summary>
    public class LibraryParameter
    {
        /// <summary>
        /// An empty parameter list.
        /// </summary>
        public static readonly LibraryParameter[] None = { };

        /// <summary>
        /// Create a <see cref="LibraryParameter"/>.
        /// </summary>
        /// <param name="name">The parameter's <see cref="Name"/>.</param>
        /// <param name="type">The parameter's <see cref="Type"/>.</param>
        public LibraryParameter(string name, LibraryTypeReference type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets the parameter's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the parameter's type.
        /// </summary>
        public LibraryTypeReference Type { get; }
    }
}
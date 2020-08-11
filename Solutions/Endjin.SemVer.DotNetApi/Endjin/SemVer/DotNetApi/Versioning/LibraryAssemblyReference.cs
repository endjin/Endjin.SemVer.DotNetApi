// <copyright file="LibraryAssemblyReference.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    /// <summary>
    /// Describes the assembly in which the type identified by a <see cref="LibraryTypeReference"/>
    /// is defined.
    /// </summary>
    public class LibraryAssemblyReference
    {
        /// <summary>
        /// Creates a <see cref="LibraryAssemblyReference"/>.
        /// </summary>
        /// <param name="shortName">The <see cref="ShortName"/>.</param>
        /// <param name="fullName">The <see cref="FullName"/>.</param>
        public LibraryAssemblyReference(
            string shortName,
            string fullName)
        {
            this.ShortName = shortName;
            this.FullName = fullName;
        }

        /// <summary>
        /// Gets the name of the assembly, e.g., <c>System.Numerics</c>.
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// Gets the full name of the assembly, including version and token.
        /// </summary>
        public string FullName { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is LibraryAssemblyReference that && this.FullName == that.FullName;
        }

        /// <inheritdoc />
        public override int GetHashCode() => this.FullName.GetHashCode();
    }
}

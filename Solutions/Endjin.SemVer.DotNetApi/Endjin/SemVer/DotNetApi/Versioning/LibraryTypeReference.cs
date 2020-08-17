// <copyright file="LibraryTypeReference.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    /// <summary>
    /// Describes the type of some library element, such as a <see cref="LibraryProperty"/> or
    /// a <see cref="LibraryParameter"/> of a <see cref="LibraryMethod"/>.
    /// </summary>
    public class LibraryTypeReference
    {
        private static readonly char[] TypeNameDelimiters = { '.', '+' };

        /// <summary>
        /// Creates a <see cref="LibraryTypeReference"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/>.</param>
        /// <param name="fullName">The <see cref="FullName"/>.</param>
        public LibraryTypeReference(LibraryAssemblyReference assembly, string fullName)
        {
            this.Assembly = assembly;
            this.FullName = fullName;

            this.Name = this.FullName.Substring(this.FullName.LastIndexOfAny(TypeNameDelimiters) + 1);
        }

        /// <summary>
        /// Gets the assembly in which the type is defined.
        /// </summary>
        public LibraryAssemblyReference Assembly { get; }

        /// <summary>
        /// Gets the full (namespace-qualified) name of the type.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets the name of the type without namespace qualification.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
        public override string ToString() => $"[{this.Assembly.ShortName}]{this.Name}";
    }
}

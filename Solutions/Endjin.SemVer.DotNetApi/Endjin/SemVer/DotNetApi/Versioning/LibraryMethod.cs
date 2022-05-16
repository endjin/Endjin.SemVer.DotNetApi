// <copyright file="LibraryMethod.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes a method which has added, removed, or changed in some way.
    /// </summary>
    public class LibraryMethod : LibraryMethodBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryMethod"/> class.
        /// </summary>
        /// <param name="name">The method's <see cref="Name"/>.</param>
        /// <param name="returnType">
        /// The method's <see cref="ReturnType"/>, or <c>null</c> if the return type is <c>void</c>.
        /// </param>
        /// <param name="parameters">
        /// The method's <see cref="LibraryMethodBase.Parameters"/>, or <c>null</c> if it has none.
        /// </param>
        public LibraryMethod(string name, LibraryTypeReference returnType = null, IReadOnlyList<LibraryParameter> parameters = null)
            : base(parameters)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.ReturnType = returnType;
        }

        /// <summary>
        /// Gets the method's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the method's return type, or null if it is <c>void</c>.
        /// </summary>
        public LibraryTypeReference ReturnType { get; }
    }
}
// <copyright file="LibraryConstructor.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes a constructor which has added, removed, or changed in some way.
    /// </summary>
    public class LibraryConstructor : LibraryMethodBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryConstructor"/> class.
        /// </summary>
        /// <param name="parameters">
        /// The constructor's <see cref="LibraryMethodBase.Parameters"/>, or <c>null</c> if it has none.
        /// </param>
        public LibraryConstructor(IReadOnlyList<LibraryParameter> parameters = null)
            : base(parameters)
        {
        }
    }
}

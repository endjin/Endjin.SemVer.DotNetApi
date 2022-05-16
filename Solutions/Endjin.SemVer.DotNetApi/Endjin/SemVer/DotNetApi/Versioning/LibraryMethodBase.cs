// <copyright file="LibraryMethodBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class of descriptions of a method-like member which has added, removed, or changed in
    /// some way.
    /// </summary>
    public abstract class LibraryMethodBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryMethodBase"/> class.
        /// </summary>
        /// <param name="parameters">
        /// The method's <see cref="Parameters"/>, or <c>null</c> if it has none.
        /// </param>
        protected LibraryMethodBase(IReadOnlyList<LibraryParameter> parameters = null)
        {
            this.Parameters = parameters ?? LibraryParameter.None;
        }

        /// <summary>
        /// Gets the method's parameters.
        /// </summary>
        public IReadOnlyList<LibraryParameter> Parameters { get; }
    }
}
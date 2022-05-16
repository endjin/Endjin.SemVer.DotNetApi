// <copyright file="SemanticVersionChangeExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    /// <summary>
    /// Extension methods for <see cref="SemanticVersionChange"/>.
    /// </summary>
    public static class SemanticVersionChangeExtensions
    {
        /// <summary>
        /// Returns the highest of two <see cref="SemanticVersionChange"/>s.
        /// </summary>
        /// <param name="left">The first <see cref="SemanticVersionChange"/>.</param>
        /// <param name="right">The second <see cref="SemanticVersionChange"/>.</param>
        /// <returns>
        /// The highest of the two <see cref="SemanticVersionChange"/>s.
        /// </returns>
        public static SemanticVersionChange AtLeast(this SemanticVersionChange left, SemanticVersionChange right) => left > right ? left : right;
    }
}
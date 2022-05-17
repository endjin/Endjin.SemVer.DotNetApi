// <copyright file="INuGetPackage.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NuGet.Frameworks;
    using NuGet.Packaging.Core;

    /// <summary>
    /// Provides access to information about a NuGet package.
    /// </summary>
    public interface INuGetPackage : IDisposable
    {
        /// <summary>
        /// Gets the NuGet identity of this package.
        /// </summary>
        PackageIdentity Identity { get; }

        /// <summary>
        /// Gets all of the library items in the package, grouped by target framework.
        /// </summary>
        /// <returns>
        /// A dictionary mapping target framework to lists of library items.
        /// </returns>
        Task<Dictionary<NuGetFramework, List<string>>> GetLibraryItemsByFrameworkAsync();

        /// <summary>
        /// Copies items out of the package to a local folder.
        /// </summary>
        /// <param name="folder">The folder to copy the files to.</param>
        /// <param name="allItems">The items to copy.</param>
        /// <returns>A task that completes when the copy has been performed.</returns>
        Task CopyFilesAsync(string folder, List<string> allItems);
    }
}
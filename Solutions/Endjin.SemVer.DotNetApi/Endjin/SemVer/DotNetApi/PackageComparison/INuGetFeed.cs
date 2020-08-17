// <copyright file="INuGetFeed.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System.Threading.Tasks;
    using NuGet.Packaging.Core;

    /// <summary>
    /// Provides access to information from a NuGet feed.
    /// </summary>
    public interface INuGetFeed
    {
        /// <summary>
        /// Gets information about all versions of a published library from the feed, if any exist.
        /// </summary>
        /// <param name="packageId">The NuGet package id (e.g., <c>Endin.Retry</c>).</param>
        /// <returns>
        /// A task that produces null if no packages with this id have been published, or a
        /// <see cref="INuGetPublishedLibraryVersions"/> providing information about published versions
        /// if one or more exist.
        /// </returns>
        Task<INuGetPublishedLibraryVersions> GetPublishedVersionsOfLibraryAsync(string packageId);

        /// <summary>
        /// Fetches a package from a feed and returns information about it.
        /// </summary>
        /// <param name="packageIdentity">The identity of the package required.</param>
        /// <returns>Information about the package.</returns>
        Task<INuGetPackage> GetPackageAsync(PackageIdentity packageIdentity);
    }
}

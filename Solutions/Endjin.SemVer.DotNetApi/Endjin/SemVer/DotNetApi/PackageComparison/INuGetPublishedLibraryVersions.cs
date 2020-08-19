// <copyright file="INuGetPublishedLibraryVersions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using NuGet.Versioning;

    /// <summary>
    /// Provides information about the various versions of a library published in a NuGet feed.
    /// </summary>
    public interface INuGetPublishedLibraryVersions
    {
        /// <summary>
        /// Gets the latest published version of this library.
        /// </summary>
        NuGetVersion LatestPublishedVersion { get; }

        /// <summary>
        /// Gets the highest major version number of all versions already published.
        /// </summary>
        int LatestPublishedMajorVersion { get; }

        /// <summary>
        /// Discovers, for a given major version, each minor version that has been published, and
        /// returns information about the latest published version of each such minor version.
        /// </summary>
        /// <param name="majorVersion">The major version number of interest.</param>
        /// <param name="latestMatchingMinorVersion">
        /// Information about the latest published version for each distinct minor version for
        /// which at least one version has been published.
        /// </param>
        /// <returns>
        /// True if at least one version with the specified major version has been published, false
        /// if none found.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Suppose the following versions have been published: v1.0.0, v1.0.13, v1.0.15, v1.1.2,
        /// v1.2.10, v1.2.20, v2.0.14, v2.0.25, v2.2.12, v2.3.98, v2.3.104.
        /// </para>
        /// <para>
        /// Calling this method with a <c>majorVersion</c> of 1 will return a
        /// <see cref="INuGetPublishedLibraryMinorVersions"/> reporting a
        /// <see cref="INuGetPublishedLibraryMinorVersions.LatestPublishedVersionWithSameMajor"/>
        /// of 2 (because the latest published version with a major version of 1 is v1.2.20). If
        /// you call the resulting object's
        /// <see cref="INuGetPublishedLibraryMinorVersions.TryGetLatestVersionWithMinorVersion(int, out NuGetVersion)"/>
        /// method with a <c>minorVersion</c> of 0, it will return a
        /// <c>lastestPublishedPatchVersion</c> of 15 (because the latest published version with a
        /// major.minor version of 1.0 is v1.0.15). If you pass a <c>minorVersion</c> of 1, it will
        /// report a patch version of 2 (because the latest (only) published version with a
        /// major.minor of 1.1 is v1.1.2). And if you specify a <c>minorVersion</c> of 2, it will
        /// report a patch of 10 because of v1.2.10.
        /// </para>
        /// <para>
        /// If you call this method with a <c>majorVersion</c> of 2, it will return a
        /// <see cref="INuGetPublishedLibraryMinorVersions"/> reporting a
        /// <see cref="INuGetPublishedLibraryMinorVersions.LatestPublishedVersionWithSameMajor"/>
        /// of 3 because the latest published version with a major version of 2 is v2.3.98. If you
        /// ask the <see cref="INuGetPublishedLibraryMinorVersions"/> for the latest version with a
        /// minor version of 0, it will report a patch of 25 (because of v2.0.25). If you ask it
        /// about a minor version of 1, it will report that there are no versions with that patch,
        /// for the major version of 2. If you ask it about minor versions of 2 or 3, it will
        /// report patches of 12 and 104 respectively.
        /// </para>
        /// </remarks>
        bool TryGetLatestVersionsWithMajorVersion(
            int majorVersion,
            out INuGetPublishedLibraryMinorVersions latestMatchingMinorVersion);
    }
}

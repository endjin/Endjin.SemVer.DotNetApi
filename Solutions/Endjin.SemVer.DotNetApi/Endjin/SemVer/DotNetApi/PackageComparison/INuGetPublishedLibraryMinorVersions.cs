// <copyright file="INuGetPublishedLibraryMinorVersions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using NuGet.Versioning;

    /// <summary>
    /// Information about the versions of a library already published matching a particular
    /// <see cref="MajorVersion"/>.
    /// </summary>
    public interface INuGetPublishedLibraryMinorVersions
    {
        /// <summary>
        /// Gets the identity of the published package with the highest minor version number of
        /// all versions that share <see cref="MajorVersion"/>.
        /// </summary>
        NuGetVersion LatestPublishedVersionWithSameMajor { get; }

        /// <summary>
        /// Gets the latest minor version number with which a package sharing <see cref="MajorVersion"/>
        /// has been published.
        /// </summary>
        int LatestPublishedMinorVersionWithSameMajor { get; }

        /// <summary>
        /// Gets the major version number for which this object describes available minor versions.
        /// </summary>
        /// <remarks>
        /// This will be the same version number specified in the call to
        /// <see cref="INuGetPublishedLibraryVersions.TryGetLatestVersionsWithMajorVersion(int, out INuGetPublishedLibraryMinorVersions)"/>
        /// that this <see cref="INuGetPublishedLibraryMinorVersions"/> came from.
        /// </remarks>
        int MajorVersion { get; }

        /// <summary>
        /// Returns highest published version with a major version number matching
        /// <see cref="MajorVersion"/>, and with the specified minor version number, if any such
        /// version has been published.
        /// </summary>
        /// <param name="minorVersion">
        /// The minor version required.
        /// </param>
        /// <param name="lastestPublishedPatchVersion">
        /// The identity of the package with the highest version number that has been published with this instance's
        /// <see cref="MajorVersion"/> number and the specified minor version number.
        /// </param>
        /// <returns>
        /// True if at least one version with a matching major.minor version number has been published.
        /// False if no version with the relevant major.minor version number has been published.
        /// </returns>
        bool TryGetLatestVersionWithMinorVersion(int minorVersion, out NuGetVersion lastestPublishedPatchVersion);
    }
}

// <copyright file="PublishedLibrary.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System.Collections.Generic;
    using System.Linq;
    using NuGet.Packaging.Core;

    /// <summary>
    /// Provides information about the various versions of a library published in a NuGet feed.
    /// </summary>
    internal class PublishedLibrary : INuGetPublishedLibraryVersions
    {
        private readonly Dictionary<int, INuGetPublishedLibraryMinorVersions> latestMinorsByMajor;

        /// <summary>
        /// Create a <see cref="PublishedLibrary"/>.
        /// </summary>
        /// <param name="publishedVersions">
        /// The versions of this library that have been published.
        /// </param>
        public PublishedLibrary(IEnumerable<PackageIdentity> publishedVersions)
        {
            this.latestMinorsByMajor = publishedVersions
                .GroupBy(v => v.Version.Major)
                .ToDictionary(
                    g => g.Key,
                    g => (INuGetPublishedLibraryMinorVersions)new MinorVersions(g));

            this.LatestPublishedVersion = publishedVersions.Max();
        }

        /// <summary>
        /// Gets the identity of the latest published version of this library.
        /// </summary>
        public PackageIdentity LatestPublishedVersion { get; }

        /// <summary>
        /// Gets the highest major version number of all versions already published.
        /// </summary>
        public int LatestPublishedMajorVersion => this.LatestPublishedVersion.Version.Major;

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
        /// Calling this method with a <c>majorVersion</c> of 1 will return a <see cref="MinorVersions"/>
        /// reporting a <see cref="MinorVersions.LatestPublishedVersionWithSameMajor"/> of 2 (because the latest
        /// published version with a major version of 1 is v1.2.20). If you call the resulting object's
        /// <see cref="MinorVersions.TryGetLatestVersionWithMinorVersion(int, out PackageIdentity)"/> method with a
        /// <c>minorVersion</c> of 0, it will return a <c>lastestPublishedPatchVersion</c> of 15 (because
        /// the latest published version with a major.minor version of 1.0 is v1.0.15). If you pass a
        /// <c>minorVersion</c> of 1, it will report a patch version of 2 (because the latest (only)
        /// published version with a major.minor of 1.1 is v1.1.2). And if you specify a <c>minorVersion</c>
        /// of 2, it will report a patch of 10 because of v1.2.10.
        /// </para>
        /// <para>
        /// If you call this method with a <c>majorVersion</c> of 2, it will return a <see cref="MinorVersions"/>
        /// reporting a <see cref="MinorVersions.LatestPublishedVersionWithSameMajor"/> of 3 because the latest
        /// published version with a major version of 2 is v2.3.98. If you ask the <see cref="MinorVersions"/>
        /// for the latest version with a minor version of 0, it will report a patch of 25 (because of v2.0.25).
        /// If you ask it about a minor version of 1, it will report that there are no versions with that patch,
        /// for the major version of 2. If you ask it about minor versions of 2 or 3, it will report patches of
        /// 12 and 104 respectively.
        /// </para>
        /// </remarks>
        public bool TryGetLatestVersionsWithMajorVersion(int majorVersion, out INuGetPublishedLibraryMinorVersions latestMatchingMinorVersion)
            => this.latestMinorsByMajor.TryGetValue(majorVersion, out latestMatchingMinorVersion);

        /// <summary>
        /// Provides information about the latest published minor versions for a particular major
        /// version number.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is returned by <see cref="TryGetLatestVersionsWithMajorVersion(int, out INuGetPublishedLibraryMinorVersions)"/>.
        /// </para>
        /// </remarks>
        public class MinorVersions : INuGetPublishedLibraryMinorVersions
        {
            private readonly Dictionary<int, PackageIdentity> latestPatchesByMinor;

            /// <summary>
            /// Create a <see cref="MinorVersions"/>.
            /// </summary>
            /// <param name="publishedVersions">
            /// The minor versions that have been published.
            /// </param>
            internal MinorVersions(
                IEnumerable<PackageIdentity> publishedVersions)
            {
                this.latestPatchesByMinor = publishedVersions
                    .GroupBy(v => v.Version.Minor)
                    .ToDictionary(
                        g => g.Key,
                        g => g.MaxBy(v => v.Version.Patch).Single());

                this.LatestPublishedVersionWithSameMajor = this.latestPatchesByMinor[this.latestPatchesByMinor.Keys.Max()];
            }

            /// <summary>
            /// Gets the identity of the published package with the highest minor version number of
            /// all versions that share <see cref="MajorVersion"/>.
            /// </summary>
            public PackageIdentity LatestPublishedVersionWithSameMajor { get; }

            /// <summary>
            /// Gets the latest minor version number with which a package sharing <see cref="MajorVersion"/>
            /// has been published.
            /// </summary>
            public int LatestPublishedMinorVersionWithSameMajor => this.LatestPublishedVersionWithSameMajor.Version.Minor;

            /// <summary>
            /// Gets the major version number for which this object describes available minor versions.
            /// </summary>
            /// <remarks>
            /// This will be the same version number specified in the call to
            /// <see cref="TryGetLatestVersionsWithMajorVersion(int, out INuGetPublishedLibraryMinorVersions)"/> that this
            /// <see cref="MinorVersions"/> came from.
            /// </remarks>
            public int MajorVersion => this.LatestPublishedVersionWithSameMajor.Version.Major;

            /// <summary>
            /// Returns the patch number of highest published version with a major version number
            /// matching <see cref="MajorVersion"/>, and with
            /// the specified minor version number, if any such version has been published.
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
            public bool TryGetLatestVersionWithMinorVersion(int minorVersion, out PackageIdentity lastestPublishedPatchVersion)
                => this.latestPatchesByMinor.TryGetValue(minorVersion, out lastestPublishedPatchVersion);
        }
    }
}

// <copyright file="PublishedLibrary.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System.Collections.Generic;
    using System.Linq;
    using NuGet.Versioning;

    /// <summary>
    /// Provides information about the various versions of a library published in a NuGet feed.
    /// </summary>
    internal class PublishedLibrary : INuGetPublishedLibraryVersions
    {
        private readonly Dictionary<int, INuGetPublishedLibraryMinorVersions> latestMinorsByMajor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedLibrary"/> class.
        /// </summary>
        /// <param name="publishedVersions">
        /// The versions of this library that have been published.
        /// </param>
        public PublishedLibrary(IEnumerable<NuGetVersion> publishedVersions)
        {
            this.latestMinorsByMajor = publishedVersions
                .GroupBy(v => v.Major)
                .ToDictionary(
                    g => g.Key,
                    g => (INuGetPublishedLibraryMinorVersions)new MinorVersions(g));

            this.LatestPublishedVersion = publishedVersions.Max();
        }

        /// <inheritdoc/>
        public NuGetVersion LatestPublishedVersion { get; }

        /// <inheritdoc/>
        public int LatestPublishedMajorVersion => this.LatestPublishedVersion.Version.Major;

        /// <inheritdoc/>
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
        private class MinorVersions : INuGetPublishedLibraryMinorVersions
        {
            private readonly Dictionary<int, NuGetVersion> latestPatchesByMinor;

            /// <summary>
            /// Initializes a new instance of the <see cref="MinorVersions"/> class.
            /// </summary>
            /// <param name="publishedVersions">
            /// The minor versions that have been published.
            /// </param>
            internal MinorVersions(IEnumerable<NuGetVersion> publishedVersions)
            {
                this.latestPatchesByMinor = publishedVersions
                    .GroupBy(v => v.Minor)
                    .ToDictionary(
                        g => g.Key,
                        g => g.MaxByWithTies(v => v.Patch).Single());

                this.LatestPublishedVersionWithSameMajor = this.latestPatchesByMinor[this.latestPatchesByMinor.Keys.Max()];
            }

            /// <inheritdoc/>
            public NuGetVersion LatestPublishedVersionWithSameMajor { get; }

            /// <inheritdoc/>
            public int LatestPublishedMinorVersionWithSameMajor => this.LatestPublishedVersionWithSameMajor.Version.Minor;

            /// <inheritdoc/>
            public int MajorVersion => this.LatestPublishedVersionWithSameMajor.Version.Major;

            /// <inheritdoc/>
            public bool TryGetLatestVersionWithMinorVersion(int minorVersion, out NuGetVersion lastestPublishedPatchVersion)
                => this.latestPatchesByMinor.TryGetValue(minorVersion, out lastestPublishedPatchVersion);
        }
    }
}
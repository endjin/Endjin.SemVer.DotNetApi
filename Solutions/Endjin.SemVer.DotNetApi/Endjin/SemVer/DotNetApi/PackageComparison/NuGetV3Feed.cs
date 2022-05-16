// <copyright file="NuGetV3Feed.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using NuGet.Common;
    using NuGet.Configuration;
    using NuGet.Packaging;
    using NuGet.Packaging.Core;
    using NuGet.Protocol;
    using NuGet.Protocol.Core.Types;
    using NuGet.Versioning;

    /// <summary>
    /// Provides access to a NuGet V3 feed.
    /// </summary>
    internal class NuGetV3Feed : INuGetFeed
    {
        private readonly SourceCacheContext sourceCacheContext = new SourceCacheContext();
        private readonly string feedUrl;
        private readonly ILogger nuGetLogger;
        private readonly Lazy<SourceRepository> sourceRepository;
        private readonly Lazy<Task<FindPackageByIdResource>> findByIdResource;
        private readonly ISettings settings = Settings.LoadDefaultSettings(root: null);

        /// <summary>
        /// Create a <see cref="NuGetV3Feed"/>.
        /// </summary>
        /// <param name="feedUrl">The NuGet V3 feed URl.</param>
        /// <param name="nuGetLogger">The NuGet logger.</param>
        public NuGetV3Feed(string feedUrl, ILogger nuGetLogger)
        {
            this.feedUrl = feedUrl;
            this.nuGetLogger = nuGetLogger;

            this.sourceRepository = new Lazy<SourceRepository>(this.GetSourceRepository);
            this.findByIdResource = new Lazy<Task<FindPackageByIdResource>>(this.GetFindPackageByIdResource);
        }

        /// <inheritdoc/>
        public async Task<INuGetPublishedLibraryVersions> GetPublishedVersionsOfLibraryAsync(string packageId)
        {
            FindPackageByIdResource findResource = await this.findByIdResource.Value;

            IEnumerable<NuGetVersion> allVersions = await findResource.GetAllVersionsAsync(
                packageId,
                this.sourceCacheContext,
                logger: this.nuGetLogger,
                cancellationToken: CancellationToken.None).ConfigureAwait(false);

            var packageVersions = new List<NuGetVersion>();

            IEnumerable<NuGetVersion> nonPrereleaseVersions = allVersions
                .Where(v => !v.IsPrerelease);

            foreach (NuGetVersion version in nonPrereleaseVersions)
            {
                Console.WriteLine($"Item in feed: {packageId}.{version}");
                packageVersions.Add(version);
            }

            return packageVersions.Count == 0
                ? null
                : new PublishedLibrary(packageVersions);
        }

        /// <inheritdoc/>
        public async Task<INuGetPackage> GetPackageAsync(PackageIdentity packageIdentity)
        {
            DownloadResource downloadResource = await this.sourceRepository.Value.GetResourceAsync<DownloadResource>(CancellationToken.None)
                .ConfigureAwait(false);

            DownloadResourceResult downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                packageIdentity,
                new PackageDownloadContext(this.sourceCacheContext),
                SettingsUtility.GetGlobalPackagesFolder(this.settings),
                this.nuGetLogger,
                CancellationToken.None)
                .ConfigureAwait(false);

            return new PackageFromFeed(packageIdentity, downloadResult, this.sourceCacheContext, this.nuGetLogger);
        }

        private SourceRepository GetSourceRepository()
        {
            return Repository.Factory.GetCoreV3(this.feedUrl);
        }

        private Task<FindPackageByIdResource> GetFindPackageByIdResource()
        {
            // It's not clear whether we still need to do this lazy once-only initialize. Back when
            // we were using ListResource, it appeared to be important to fetch it only once,
            // because otherwise, the second attempt would destroy any cached token for this
            // endpoint!
            // As far as I can tell, what's happening is that there's some flag that tracks
            // the "we already tried using these credentials once" condition, with the intention
            // being that if you have to make a second attempt, presumably the cached credentials
            // you had were no good and need to be overwritten. It tells the credential provider
            // plugin this by passing a flag telling it that the request is a retry, and that seems
            // to be what causes the existing token to be junked.
            // The problem is that this flag doesn't get reset in the event of a successful request
            // so it wrongly interprets any second fetch of a resource as necessarily being a retry
            // caused by an earlier failure.
            // The solution is not to repeat requests unnecessarily. (This is good practice anyway
            // because it's wasteful to repeat requests.)
            // In fact we never even established whether the original problem was around getting
            // the ListResource or the SourceRepository, so we don't know whether we definitely
            // need to make this lazy.
            return this.sourceRepository.Value.GetResourceAsync<FindPackageByIdResource>();
        }

        private class PackageFromFeed : NuGetPackageBase
        {
            private readonly DownloadResourceResult downloadResult;
            private readonly SourceCacheContext cacheContext;

            public PackageFromFeed(PackageIdentity identity, DownloadResourceResult downloadResult, SourceCacheContext cacheContext, ILogger nuGetLogger)
                : base(identity, nuGetLogger)
            {
                this.downloadResult = downloadResult;
                this.cacheContext = cacheContext;
            }

            public override void Dispose()
            {
                this.downloadResult.Dispose();

                // The cache context was created specially for us. It's not quite clear to me what
                // it's for, and empirically it doesn't actually seem to matter if we dispose it
                // before we've finished with the DownloadResourceResult associated with it.
                // However, that seems like it can't be a good idea, so we dispose it only when we
                // know we're done with the files we asked to download associated with that context.
                this.cacheContext.Dispose();
            }

            protected override PackageReaderBase GetPackageReader() => this.downloadResult.PackageReader;
        }
    }
}
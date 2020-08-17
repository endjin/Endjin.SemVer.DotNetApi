// <copyright file="NuGetV3Feed.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NuGet.Common;
    using NuGet.Configuration;
    using NuGet.Packaging;
    using NuGet.Packaging.Core;
    using NuGet.Protocol;
    using NuGet.Protocol.Core.Types;

    /// <summary>
    /// Provides access to a NuGet V3 feed.
    /// </summary>
    internal class NuGetV3Feed : INuGetFeed
    {
        private readonly string feedUrl;
        private readonly ILogger nuGetLogger;
        private readonly Lazy<SourceRepository> sourceRepository;
        private readonly Lazy<ListResource> listResource;
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
            this.listResource = new Lazy<ListResource>(this.GetListResource);
        }

        /// <inheritdoc/>
        public async Task<INuGetPublishedLibraryVersions> GetPublishedVersionsOfLibraryAsync(string packageId)
        {
            ListResource listResource = this.listResource.Value;

            IEnumerableAsync<IPackageSearchMetadata> x = await listResource.ListAsync(
                packageId,
                prerelease: false,
                allVersions: true,
                includeDelisted: false,
                log: this.nuGetLogger,
                token: CancellationToken.None).ConfigureAwait(false);

            var packageVersions = new List<PackageIdentity>();

            IEnumeratorAsync<IPackageSearchMetadata> e = x.GetEnumeratorAsync();

            while (await e.MoveNextAsync().ConfigureAwait(false))
            {
                PackageIdentity packageIdentity = e.Current.Identity;
                Console.WriteLine("Item in feed: " + packageIdentity);

                if (packageIdentity.Id == packageId)
                {
                    packageVersions.Add(packageIdentity);
                }
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

            var cacheContext = new SourceCacheContext();

            DownloadResourceResult downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                packageIdentity,
                new PackageDownloadContext(cacheContext),
                SettingsUtility.GetGlobalPackagesFolder(this.settings),
                this.nuGetLogger,
                CancellationToken.None)
                .ConfigureAwait(false);

            return new PackageFromFeed(packageIdentity, downloadResult, cacheContext, this.nuGetLogger);
        }

        private SourceRepository GetSourceRepository()
        {
            return Repository.Factory.GetCoreV3(this.feedUrl);
        }

        private ListResource GetListResource()
        {
            // It appears to be important that we fetch this ListResource only once, because
            // otherwise, the second attempt will destroy any cached token for this endpoint!
            // As far as I can tell, what's happening is that there's some flag that tracks
            // the "we already tried using these credentials once" condition, with the intention
            // being that if you have to make a second attempt, presumably the cached credentials
            // you had were no good and need to be overwritten. It tells the credential provider
            // plugin this by passing a flag telling it that the request is a retry, and that seems
            // to be what causes the existing token to be junked.
            // The proplem is that this flag doesn't get reset in the event of a successful request
            // so it wrongly interprets any second fetch of a resource as necessarily being a retry
            // caused by an earlier failure.
            // The solution is not to repeat requests unnecessarily. (This is good practice anyway
            // because it's wasteful to repeat requests.)
            // TODO: is it getting the ListResource once that's the issue? Or getting the SourceRepository?
            return this.sourceRepository.Value.GetResource<ListResource>();
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

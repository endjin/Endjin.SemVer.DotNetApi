// <copyright file="INuGetFeedFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    /// <summary>
    /// A source of <see cref="INuGetFeed"/> instances.
    /// </summary>
    public interface INuGetFeedFactory
    {
        /// <summary>
        /// Gets a feed for the specified NuGet v3 feed URL.
        /// </summary>
        /// <param name="feedUrl">NuGet feed URL.</param>
        /// <returns>
        /// A <see cref="INuGetFeed"/> providing the ability to query the feed and download
        /// packages.
        /// </returns>
        INuGetFeed GetV3Feed(string feedUrl);
    }
}
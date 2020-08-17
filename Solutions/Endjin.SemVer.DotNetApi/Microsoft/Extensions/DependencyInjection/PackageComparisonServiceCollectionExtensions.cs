// <copyright file="PackageComparisonServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Endjin.SemVer.DotNetApi.PackageComparison;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// DI configuration for package comparison services.
    /// </summary>
    public static class PackageComparisonServiceCollectionExtensions
    {
        /// <summary>
        /// Add services required to perform package comparisons.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="allowInteractiveNuGetAuthentication">
        /// Determines whether interactive authentication may be attempted when a NuGet feed
        /// requires authentication and no cached credentials are found.
        /// </param>
        /// <returns>The modified service collection.</returns>
        public static IServiceCollection AddPackageComparison(
            this IServiceCollection services,
            bool allowInteractiveNuGetAuthentication)
        {
            return services
                .AddSingleton<IPackageCollectionComparisonOrchestrator, PackageCollectionComparisonOrchestrator>()
                .AddSingleton<INuGetFeedFactory>(sp => new NuGetFeedFactory(sp, allowInteractiveNuGetAuthentication))
                .AddSingleton<INuGetLocalPackageEnumerator, NuGetLocalPackageEnumerator>()
                .AddSingleton<NuGet.Common.ILogger, NuGetLoggerAdapter>();
        }
    }
}

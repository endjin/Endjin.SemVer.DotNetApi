// <copyright file="NuGetFeedFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using NuGet.Credentials;

    /// <summary>
    /// Provides access to information from NuGet feeds.
    /// </summary>
    internal class NuGetFeedFactory : INuGetFeedFactory
    {
        private readonly NuGet.Common.ILogger nuGetLogger;

        /// <summary>
        /// Create a <see cref="NuGetFeedFactory"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// DI service provider.
        /// </param>
        /// <param name="allowInteractiveAuthentication">
        /// Determines whether interactive authentication may be attempted when a NuGet feed
        /// requires authentication and no cached credentials are found.
        /// </param>
        public NuGetFeedFactory(
            IServiceProvider serviceProvider,
            bool allowInteractiveAuthentication)
        {
            this.nuGetLogger = serviceProvider.GetRequiredService<NuGet.Common.ILogger>();
            this.UseDefaultCredentials(allowInteractiveAuthentication);
        }

        /// <inheritdoc/>
        public INuGetFeed GetV3Feed(string feedUrl)
        {
            return new NuGetV3Feed(feedUrl, this.nuGetLogger);
        }

        private void UseDefaultCredentials(bool allowInteractiveAuthentication)
        {
            // This enables us to pick up credentials through plugins. E.g., if the Azure Artifacts Credential Provider
            // from https://github.com/Microsoft/artifacts-credprovider has been installed (and that's the recommended
            // mechanism for authenticating with Azure DevOps package feeds), this will pick it up.
            // If you're running locally, and you've previously authenticated, e.g. with
            //  dotnet restore --interactive MyProj.csproj
            // against a project that uses the same feed, and if the token thus acquired is still valid, we will be
            // able to pick up that token automatically.
            // Note that dotnet restore doesn't always hit the package endpoint. If you have a project in which all
            // packages have already been restored, and are in your local .nuget package store, dotnet restore will
            // decide that it doesn't have to do anything. And even if you specify the --force and --no-cache options,
            // it will still decide that it doesn't need to hit the feed endpoint if all relevant packages are in
            // your local .nuget folder. (This is not considered a 'cache' in the terminology of the command line tools.
            // It is a package source.)
            // If you want to force a download, you need to Clean the project to remove any cached NuGet work in the
            // project bin or obj folders, then create an empty folder, and then run this command:
            //  dotnet restore --packages c:\temp\MyEmptyFolder MyProj.csproj
            // This tells it to use your empty folder as the local package store, and this will stop it from using
            // the shared .nuget folder in your profile. This means that it will finally have no choice but to
            // download the package from the feed, at which point this will trigger authentication.
            DefaultCredentialServiceUtility.SetupDefaultCredentialService(
                this.nuGetLogger,
                nonInteractive: !allowInteractiveAuthentication);
        }
    }
}

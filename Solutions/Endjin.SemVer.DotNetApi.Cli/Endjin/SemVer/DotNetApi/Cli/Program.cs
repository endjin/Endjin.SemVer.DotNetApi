// <copyright file="Program.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Cli
{
    using System.Threading.Tasks;
    using Endjin.SemVer.DotNetApi.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Api Compare tool entry point.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Program entry point, called by System.CommandLine DragonFruit.
        /// </summary>
        /// <param name="feedUrl">
        /// The NuGet package feed URL from which to fetch previously published packages.
        /// </param>
        /// <param name="packageFolder">
        /// The path to the local folder containing the newly-built packages to compare with
        /// previously published packages.
        /// </param>
        /// <param name="interactive">
        /// Enables login prompts if the feed requires authentication and no credentials can be found.
        /// </param>
        /// <param name="verbosity">
        /// Trace, Debug, Information, Warning, or Error.
        /// </param>
        /// <returns>
        /// A task that completes when the application has finished.
        /// </returns>
        internal static async Task<int> Main(
            string feedUrl,
            string packageFolder,
            bool interactive = false,
            string verbosity = null)
        {
            var arguments = new ApiCompareCommandArguments(feedUrl, packageFolder);

            LogLevel logLevel = LogLevel.Warning;
            if (verbosity != null)
            {
                switch (verbosity)
                {
                    case "Trace": logLevel = LogLevel.Trace; break;
                    case "Debug": logLevel = LogLevel.Debug; break;
                    case "Information": logLevel = LogLevel.Information; break;
                    case "Warning": logLevel = LogLevel.Warning; break;
                    case "Error": logLevel = LogLevel.Error; break;
                }
            }

            IHostBuilder builder = new HostBuilder()
                .ConfigureServices((_, services) =>
                {
                    services
                        .Configure<LoggerFilterOptions>(o => o.MinLevel = logLevel)
                        .AddSingleton<IHostedCommand, ApiCompareTool>()
                        .AddSingleton(arguments)
                        .AddPackageComparison(interactive);
                })
                .ConfigureLogging(logging => logging.AddConsole());

            return await builder.RunInHostedCommandModeAsync();
        }
    }
}

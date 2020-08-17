// <copyright file="ComparePackagesHandler.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Cli
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Threading.Tasks;

    using Endjin.SemVer.DotNetApi.PackageComparison;

    using Microsoft.Extensions.Logging;

    public static class ComparePackagesHandler
    {
        public static async Task<int> ExecuteAsync(
            IPackageCollectionComparisonOrchestrator orchestrator,
            ILogger logger,
            CompareArguments arguments,
            IConsole console,
            InvocationContext context = null)
        {
            try
            {
                bool ok = await orchestrator.CompareAsync(arguments.PackageFeedUrl, arguments.PackageDirectory).ConfigureAwait(false);
                return ok ? 0 : 1;
            }
            catch (Exception x)
            {
                logger.LogError(x.ToString());
                return 1;
            }
        }
    }
}

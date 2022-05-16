// <copyright file="ApiCompareTool.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Cli
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Endjin.SemVer.DotNetApi.Hosting;
    using Endjin.SemVer.DotNetApi.PackageComparison;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// API comparison command line tool.
    /// </summary>
    /// <remarks>
    /// This class's job is to adapt the underlying comparison logic to the hosting system - its
    /// only jobs are to implement <see cref="IHostedCommand"/> and to plumb the relevant settings
    /// from the command line arguments into the code that does the actual work.
    /// </remarks>
    internal class ApiCompareTool : IHostedCommand
    {
        private readonly CompareArguments arguments;
        private readonly IPackageCollectionComparisonOrchestrator orchestrator;
        private readonly ILogger<ApiCompareTool> logger;

        /// <summary>
        /// Create a <see cref="ApiCompareTool"/>.
        /// </summary>
        /// <param name="arguments">The command line arguments.</param>
        /// <param name="orchestrator">The orchestrator that does the underlying work.</param>
        /// <param name="logger">The logger.</param>
        public ApiCompareTool(
            CompareArguments arguments,
            IPackageCollectionComparisonOrchestrator orchestrator,
            ILogger<ApiCompareTool> logger)
        {
            this.arguments = arguments;
            this.orchestrator = orchestrator;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<int> RunAsync(CancellationToken token)
        {
            this.logger.LogInformation("Hello, world!");

            try
            {
                bool ok = await this.orchestrator.CompareAsync(this.arguments.PackageFeedUrl, this.arguments.PackageDirectory);
                return ok ? 0 : 1;
            }
            catch (Exception x)
            {
                this.logger.LogError(x.ToString());
                return 1;
            }
        }
    }
}
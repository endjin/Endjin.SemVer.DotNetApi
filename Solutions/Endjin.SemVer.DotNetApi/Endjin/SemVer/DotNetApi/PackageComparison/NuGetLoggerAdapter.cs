// <copyright file="NuGetLoggerAdapter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using ILogMessage = NuGet.Common.ILogMessage;

    /// <summary>
    /// Routes NuGet log output through to the .NET Core logging system.
    /// </summary>
    internal class NuGetLoggerAdapter : NuGet.Common.LoggerBase
    {
        private readonly ILogger<NuGetLoggerAdapter> logger;

        /// <summary>
        /// Create a <see cref="NuGetLoggerAdapter"/>.
        /// </summary>
        /// <param name="logger">The underlying logger.</param>
        public NuGetLoggerAdapter(ILogger<NuGetLoggerAdapter> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public override void Log(ILogMessage message)
        {
            this.logger.Log(MapLevel(message.Level), message.Message);
        }

        /// <inheritdoc/>
        public override Task LogAsync(ILogMessage message)
        {
            this.Log(message);
            return Task.CompletedTask;
        }

        private static LogLevel MapLevel(NuGet.Common.LogLevel level)
        {
            switch (level)
            {
                case NuGet.Common.LogLevel.Debug:
                    return LogLevel.Trace;
                case NuGet.Common.LogLevel.Verbose:
                    return LogLevel.Debug;
                case NuGet.Common.LogLevel.Information:
                    return LogLevel.Information;
                case NuGet.Common.LogLevel.Minimal:
                    return LogLevel.Warning;
                case NuGet.Common.LogLevel.Warning:
                    return LogLevel.Warning;
                case NuGet.Common.LogLevel.Error:
                    return LogLevel.Error;
                default:
                    throw new ArgumentException($"Unknown log level {level}", nameof(level));
            }
        }
    }
}
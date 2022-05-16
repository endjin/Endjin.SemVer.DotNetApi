// <copyright file="CompareArguments.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Cli
{
    using System;
    using System.IO;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The command line arguments for the compare tool.
    /// </summary>
    public class CompareArguments
    {
        private string verbosity;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareArguments"/> class.
        /// </summary>
        /// <param name="packageFeedUrl">The NuGet <see cref="PackageFeedUrl"/>.</param>
        /// <param name="packageDirectory">The <see cref="PackageDirectory"/>.</param>
        /// <param name="verbosity">Logging verbosity.</param>
        /// <param name="interactive">Whether to do interactive authentication for NuGet.</param>
        public CompareArguments(Uri packageFeedUrl, DirectoryInfo packageDirectory, string verbosity, bool interactive = false)
        {
            this.PackageFeedUrl = packageFeedUrl;
            this.Interactive = interactive;
            this.PackageDirectory = packageDirectory;
            this.verbosity = verbosity;
        }

        /// <summary>
        /// Gets the URL of the NuGet feed to search for earlier versions.
        /// </summary>
        public Uri PackageFeedUrl { get; }

        /// <summary>
        /// Gets a value indicating whether interactive authentication of NuGet should occur.
        /// </summary>
        public bool Interactive { get; }

        /// <summary>
        /// Gets the path of the local folder containing the newly-built packages, which are to be
        /// compared with published earlier versions.
        /// </summary>
        public DirectoryInfo PackageDirectory { get; }

        /// <summary>
        /// Gets the Logging Verbosity level.
        /// </summary>
        public LogLevel Verbosity
        {
            get
            {
                LogLevel logLevel = LogLevel.Warning;

                if (this.verbosity != null)
                {
                    switch (this.verbosity)
                    {
                        case "Trace": logLevel = LogLevel.Trace; break;
                        case "Debug": logLevel = LogLevel.Debug; break;
                        case "Information": logLevel = LogLevel.Information; break;
                        case "Warning": logLevel = LogLevel.Warning; break;
                        case "Error": logLevel = LogLevel.Error; break;
                    }
                }

                return logLevel;
            }
        }
    }
}
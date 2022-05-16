// <copyright file="CommandResultCollector.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Hosting
{
    /// <summary>
    /// Enables the result of an <see cref="IHostedCommand"/> to be retrieved after execution
    /// completes, so that it can be returned as the application exit code if required.
    /// </summary>
    internal class CommandResultCollector : ICommandResultCollector
    {
        /// <summary>
        /// Gets the exit code.
        /// </summary>
        public int ExitCode { get; private set; }

        /// <inheritdoc/>
        public void SetExitCode(int exitCode)
        {
            this.ExitCode = exitCode;
        }
    }
}
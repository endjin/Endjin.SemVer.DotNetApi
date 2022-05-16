// <copyright file="ICommandResultCollector.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Hosting
{
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Receives the result from a <see cref="IHostedCommand"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Commands are executed by the <see cref="HostedCommandRunnerService"/>, which in turn is
    /// run by the <c>HostBuilder</c>. Since <c>HostBuilder</c> is designed to host long-running
    /// services, it doesn't have any built-in concept of the 'result' of a service: a service is
    /// an ongoing entity that runs indefinitely. So although the <c>RunAsync()</c> extension
    /// method for <see cref="IHost"/> that is typically used to start the host returns a task that
    /// completes when the host is shut down, this task doesn't produce any result. So we need to
    /// introduce a mechanism by which the code that invokes
    /// <see cref="IHostedCommand.RunAsync(System.Threading.CancellationToken)"/> can put the exit
    /// code produced by that method somewhere that can later be retrieved once the call
    /// to <c>host.RunAsync()</c> performed by
    /// <see cref="HostBuilderExtensions.RunInHostedCommandModeAsync(IHostBuilder)"/> completes.
    /// </para>
    /// </remarks>
    internal interface ICommandResultCollector
    {
        /// <summary>
        /// Set the exit code.
        /// </summary>
        /// <param name="exitCode">The exit code.</param>
        void SetExitCode(int exitCode);
    }
}
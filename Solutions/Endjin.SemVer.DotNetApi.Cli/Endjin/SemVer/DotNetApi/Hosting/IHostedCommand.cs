// <copyright file="IHostedCommand.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Hosting
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// An operation that runs in a Microsoft.Extensions.Hosting environment which performs some
    /// finite amount of work and then completes, at which point we want the hosting process to
    /// exit.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The services that <c>HostBuilder</c> offers—configuration, DI, and logging setup—are also
    /// of interest to command line tools of the kind that perform some job and then immediately
    /// exit. This interface represents this much simpler lifecycle, enabling command line tools
    /// to use <c>HostBuilder</c> without having to work around concerns relevant only to
    /// long-running services.
    /// </para>
    /// <para>
    /// <c>HostBuilder</c> provides direct support only for long-running services: these implement
    /// <see cref="IHostedService"/>, in which services are started, occasionally offered the
    /// opportunity to run background work, and are eventually notified when the host will be
    /// shutting down.
    /// </para>
    /// <para>
    /// To enable short-lived command-like operations also to enjoy the benefits of running in an
    /// environment set up by <see cref="HostBuilder"/>, your DI initialization must register your
    /// implementation of <see cref="IHostedCommand"/>s. Then, instead of using the
    /// <c>RunConsoleAsync</c> extension method for <see cref="IHostBuilder"/> supplied by
    /// Microsoft's libraries, you should instead use the
    /// <see cref="HostBuilderExtensions.RunInHostedCommandModeAsync(IHostBuilder)"/> method.
    /// </para>
    /// </remarks>
    public interface IHostedCommand
    {
        /// <summary>
        /// Run the command.
        /// </summary>
        /// <param name="token">
        /// A cancellation token that will be set when the host environment starts shutting down.
        /// This provides the command with the opportunity to handle early termination cleanly.
        /// This token will be signalled if the user presses Ctrl+C while the command is in
        /// progress, for example (because the <c>ConsoleLifetimeService</c> that
        /// <c>HostBuilder</c> uses tries to perform a clean shutdown when Ctrl+C is pressed).
        /// </param>
        /// <returns>
        /// A task that produces the an exit code once the command completes. This becomes the
        /// <see cref="HostBuilderExtensions.RunInHostedCommandModeAsync(IHostBuilder)"/> method's
        /// result. Applications that want to can return this as the process exit code. (The
        /// framework for running the command does nothing with this exit code besides returning
        /// it, so returning a non-zero value will not change the behaviour in any way.)
        /// </returns>
        Task<int> RunAsync(CancellationToken token);
    }
}
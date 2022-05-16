// <copyright file="HostedCommandRunnerService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Hosting
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Implements the behaviour offered by
    /// <see cref="HostBuilderExtensions.UseHostedCommandMode(IHostBuilder)"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A hosted service that waits until the host environment has fully started, and then runs the
    /// registered <see cref="IHostedCommand"/>. As soon as the command completes, this service
    /// asks the hosting environment to shut down the application.
    /// </para>
    /// </remarks>
    internal class HostedCommandRunnerService : IHostedService
    {
        private readonly IHostedCommand command;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly ICommandResultCollector resultCollector;
        private CancellationTokenSource commandCancellationTokenSource;
        private Task runCommandsTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostedCommandRunnerService"/> class.
        /// </summary>
        /// <param name="command">The command to run.</param>
        /// <param name="applicationLifetime">Enables us to request shutdown.</param>
        /// <param name="resultCollector">
        /// Holds the exit code from the command, making it possible to return it as the
        /// application exit code if required.
        /// </param>
        public HostedCommandRunnerService(
            IHostedCommand command,
            IHostApplicationLifetime applicationLifetime,
            ICommandResultCollector resultCollector)
        {
            this.command = command;
            this.applicationLifetime = applicationLifetime;
            this.resultCollector = resultCollector;
        }

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.commandCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            this.applicationLifetime.ApplicationStarted.Register(
                this.OnStarted,
                useSynchronizationContext: true);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.commandCancellationTokenSource.Cancel();
            return this.runCommandsTask ?? Task.CompletedTask;
        }

        private void OnStarted()
        {
            this.runCommandsTask = this.RunCommandsAfterStarted();
        }

        private async Task RunCommandsAfterStarted()
        {
            // Using ConfigureAwait(true) in case the application has strong feelings about
            // which synchronization context its command runs in.
            int result = await this.command.RunAsync(this.commandCancellationTokenSource.Token)
                .ConfigureAwait(true);
            this.resultCollector.SetExitCode(result);

            // Note that this is a request to shut down. It won't block until shutdown completes.
            // And that's good, because shutdown cannot complete cleanly if we're still running.
            this.applicationLifetime.StopApplication();
        }
    }
}
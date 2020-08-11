// <copyright file="HostBuilderExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Hosting
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Extension methods for <see cref="IHostBuilder"/>.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Operate in hosted command mode. Appropriate for classic command line tools that perform
        /// a task and then exit.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
        /// <returns>The modified builder.</returns>
        /// <remarks>
        /// <para>
        /// Normally, <see cref="HostBuilder"/> operates in long-running mode: it starts all
        /// <see cref="IHostedService"/> implementations registered with DI, periodically offers
        /// them the opportunity to perform background work, and only shuts things down if
        /// explicitly asked to. (Shutdown can be triggered programmatically. Additionally, the
        /// <c>ConsoleLifetime</c> class, which is the default <see cref="IHostLifetime"/> when
        /// using the non-web <c>HostBuilder</c>, which you get whether you ask for it or not -
        /// https://github.com/aspnet/Extensions/issues/574 - monitors for process termination
        /// requests, so that Ctrl-C causes an orderly shutdown.)
        /// </para>
        /// <para>
        /// Most command line tools don't want this indefinite lifetime. Instead, they typically do
        /// whatever job they've been asked to do and then exit. Nonetheless, the features offered
        /// by <c>HostBuilder</c> can be useful to such application—configuration, DI, and logging
        /// are often useful even if you're not writing a long-running service. This method enables
        /// a lifecycle more suitable to command line tools while still using <c>HostBuilder</c>.
        /// </para>
        /// <para>
        /// As a result of calling this method, each registered <see cref="IHostedCommand"/> will
        /// be executed immediately after the host has finished starting up. (It is recommended
        /// that you register just one, because there is no guarantee as to which order they will
        /// run in. But if you do register more than one, they will be executed one at a time, in
        /// no particular order.) As soon as the last of these commands finishes, an we ask the
        /// <see cref="IApplicationLifetime"/> to begin an orderly shutdown of the hosting
        /// environment, which will cause the application to exit immediately after all cleanup
        /// work completes.
        /// </para>
        /// </remarks>
        public static IHostBuilder UseHostedCommandMode(this IHostBuilder builder)
        {
            return builder.UseHostedCommandMode(new CommandResultCollector());
        }

        /// <summary>
        /// Run in hosted command mode. See <see cref="UseHostedCommandMode(IHostBuilder)"/> for
        /// details about what this means.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
        /// <returns>A task the produces the application exit code.</returns>
        public static async Task<int> RunInHostedCommandModeAsync(this IHostBuilder builder)
        {
            var resultCollector = new CommandResultCollector();
            builder.UseHostedCommandMode(resultCollector);
            await builder.Build().RunAsync();
            return resultCollector.ExitCode;
        }

        private static IHostBuilder UseHostedCommandMode(
            this IHostBuilder builder,
            ICommandResultCollector resultCollector)
        {
            return builder.ConfigureServices((_, services) => services
                .Configure<ConsoleLifetimeOptions>(o => o.SuppressStatusMessages = true)
                .AddSingleton<IHostedService, HostedCommandRunnerService>()
                .AddSingleton(resultCollector));
        }
    }
}

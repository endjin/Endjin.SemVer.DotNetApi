// <copyright file="CommandLineParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Cli
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Invocation;
    using System.CommandLine.Parsing;
    using System.IO;
    using System.Threading.Tasks;

    using Endjin.SemVer.DotNetApi.PackageComparison;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class CommandLineParser
    {
        private readonly IServiceCollection services;

        public CommandLineParser(IServiceCollection services)
        {
            this.services = services;
        }

        public delegate Task ComparePackages(IPackageCollectionComparisonOrchestrator orchestrator, ILogger logger, CompareArguments args, IConsole console, InvocationContext invocationContext = null);

        public Parser Create(ComparePackages comparePackages = null)
        {
            comparePackages ??= ComparePackagesHandler.ExecuteAsync;

            RootCommand rootCommand = Root();

            rootCommand.Handler = CommandHandler.Create<CompareArguments, InvocationContext>(async (args, context) =>
            {
                this.services.AddPackageComparison(args.Interactive)
                             .AddLogging(configure => configure.AddConsole())
                             .Configure<LoggerFilterOptions>(o => o.MinLevel = args.Verbosity);

                ServiceProvider serviceProvider = this.services.BuildServiceProvider();

                await comparePackages(
                    serviceProvider.GetRequiredService<IPackageCollectionComparisonOrchestrator>(),
                    serviceProvider.GetRequiredService<ILogger<IPackageCollectionComparisonOrchestrator>>(),
                    args,
                    context.Console,
                    context).ConfigureAwait(false);
            });

            var commandBuilder = new CommandLineBuilder(rootCommand);

            return commandBuilder.UseDefaults().Build();

            RootCommand Root()
            {
                var cmd =  new RootCommand
                {
                    Name = "nupkgversion",
                    Description = "Diff two NuGet packages and generate a Semantic Version Increment [Major | Minor | Patch].",
                };

                cmd.AddArgument(new Argument<DirectoryInfo>
                {
                    Name = "package-directory",
                    Description = "Directory where the package to evaluate is located.",
                    Arity = ArgumentArity.ExactlyOne,
                });

                cmd.AddArgument(new Argument<Uri>(getDefaultValue: () => new Uri("https://api.nuget.org/v3/index.json"))
                {
                    Name = "package-feed-url",
                    Description = "Uri of the package feed to compare against. NuGet.org set by default.",
                    Arity = ArgumentArity.ExactlyOne,
                });

                cmd.AddArgument(new Argument<bool>
                {
                    Name = "interactive",
                    Description = "Whether interactive authentication for NuGet should be used.",
                    Arity = ArgumentArity.ZeroOrOne,
                });

                cmd.AddArgument(new Argument<string>
                {
                    Name = "verbosity",
                    Description = "Logging verbosity",
                    Arity = ArgumentArity.ZeroOrOne,
                });

                return cmd;
            }
        }
    }
}

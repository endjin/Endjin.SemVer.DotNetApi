// <copyright file="Program.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Cli
{
    using System;
    using System.CommandLine.Parsing;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Api Compare tool entry point.
    /// </summary>
    internal static class Program
    {
        private static readonly ServiceCollection ServiceCollection = new ServiceCollection();

        public static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            return await new CommandLineParser(ServiceCollection).Create().InvokeAsync(args).ConfigureAwait(false);
        }
    }
}
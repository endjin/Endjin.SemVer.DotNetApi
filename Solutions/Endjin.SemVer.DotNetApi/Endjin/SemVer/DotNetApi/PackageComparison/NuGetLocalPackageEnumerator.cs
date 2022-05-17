// <copyright file="NuGetLocalPackageEnumerator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NuGet.Common;
    using NuGet.Protocol;

    /// <summary>
    /// Provides the ability to enumerate over a collection of packages stored on the local file
    /// system.
    /// </summary>
    internal class NuGetLocalPackageEnumerator : INuGetLocalPackageEnumerator
    {
        private readonly ILogger nuGetLogger;

        /// <summary>
        /// Create a <see cref="NuGetLocalPackageEnumerator"/>.
        /// </summary>
        /// <param name="nuGetLogger">The NuGet-style logger.</param>
        public NuGetLocalPackageEnumerator(ILogger nuGetLogger)
        {
            this.nuGetLogger = nuGetLogger;
        }

        /// <inheritdoc/>
        public IEnumerable<INuGetPackage> GetPackages(string packageFolder)
        {
            foreach (string path in Directory.EnumerateFiles(packageFolder, "*.nupkg", SearchOption.AllDirectories))
            {
                LocalPackageInfo pi = LocalFolderUtility.GetPackage(new Uri(path), this.nuGetLogger);

                yield return new LocalNuGetPackage(pi, this.nuGetLogger);
            }
        }
    }
}
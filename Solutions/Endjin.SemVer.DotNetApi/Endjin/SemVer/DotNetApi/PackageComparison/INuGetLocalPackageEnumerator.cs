// <copyright file="INuGetLocalPackageEnumerator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides the ability to enumerate over a collection of packages stored on the local file
    /// system.
    /// </summary>
    internal interface INuGetLocalPackageEnumerator
    {
        /// <summary>
        /// Gets all of the NuGet packages in a folder.
        /// </summary>
        /// <param name="packageFolder">
        /// The path to the folder containing the NuGet packages.
        /// </param>
        /// <returns>
        /// Information about each package found.
        /// </returns>
        IEnumerable<INuGetPackage> GetPackages(string packageFolder);
    }
}

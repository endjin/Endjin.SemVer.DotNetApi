// <copyright file="IPackageCollectionComparisonOrchestrator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System.Threading.Tasks;

    /// <summary>
    /// Orchestrates the process of inspecting a directory full of NuGet packages, and working out
    /// for each of these which, if any, of the packages on a particular NuGet feed constitutes the
    /// package's predecessor, downloading that, and kicking off a comparison.
    /// </summary>
    public interface IPackageCollectionComparisonOrchestrator
    {
        /// <summary>
        /// Find all the NuGet packages in a folder, find each one's predecessor in a NuGet feed,
        /// and if found, compare the two and check for Semantic Versioning violations.
        /// </summary>
        /// <param name="feedUrl">The NuGet feed.</param>
        /// <param name="packageFolder">The path of the folder containing the new packages.</param>
        /// <returns>
        /// A task that produces true if the packages all meet rules, and false if there are any
        /// semantic versioning violations.
        /// </returns>
        Task<bool> CompareAsync(string feedUrl, string packageFolder);
    }
}

// <copyright file="PackageCollectionComparisonOrchestrator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Endjin.SemVer.DotNetApi.Versioning;
    using NuGet.Frameworks;
    using NuGet.Packaging.Core;

    /// <summary>
    /// Orchestrates the process of inspecting a directory full of NuGet packages, and working out
    /// for each of these which, if any, of the packages on a particular NuGet feed constitutes the
    /// package's predecessor, downloading that, and kicking off a comparison.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Here be dragons.
    /// </para>
    /// <para>
    /// This code is lifted more or less straight over from the prototype, and hasn't had any
    /// productionising done. So there are no tests, and it's a big bundle of poorly structured
    /// mess.
    /// </para>
    /// </remarks>
    public class PackageCollectionComparisonOrchestrator : IPackageCollectionComparisonOrchestrator
    {
        private readonly INuGetFeedFactory feedFactory;
        private readonly INuGetLocalPackageEnumerator packageEnumerator;

        /// <summary>
        /// Create a <see cref="PackageCollectionComparisonOrchestrator"/>.
        /// </summary>
        /// <param name="feedFactory">Source of NuGet feeds.</param>
        /// <param name="packageEnumerator">Enumerate local NuGet packages.</param>
        public PackageCollectionComparisonOrchestrator(INuGetFeedFactory feedFactory, INuGetLocalPackageEnumerator packageEnumerator)
        {
            this.feedFactory = feedFactory;
            this.packageEnumerator = packageEnumerator;
        }

        /// <inheritdoc/>
        public async Task<bool> CompareAsync(Uri feedUri, DirectoryInfo packageDirectory)
        {
            bool rulesViolated = false;

            INuGetFeed feed = this.feedFactory.GetV3Feed(feedUri.ToString());
            IEnumerable<INuGetPackage> localPackages = this.packageEnumerator.GetPackages(packageDirectory.FullName);

            foreach (INuGetPackage newPackage in localPackages)
            {
                PackageIdentity thisPackageId = newPackage.Identity;
                Console.WriteLine("Processing " + newPackage.Identity);

                INuGetPublishedLibraryVersions publishedVersions = await feed.GetPublishedVersionsOfLibraryAsync(newPackage.Identity.Id).ConfigureAwait(false);

                ComponentChangeType changeType;
                PackageIdentity predecessorId = null;
                PackageIdentity latestVersionWithSameMajor = null;

                if (publishedVersions == null)
                {
                    Console.WriteLine($"No versions of {newPackage.Identity} have been published before.");
                    changeType = ComponentChangeType.NewComponent;
                }
                else
                {
                    int latestPublishedMajorVersion = publishedVersions.LatestPublishedMajorVersion;

                    if (!publishedVersions.TryGetLatestVersionsWithMajorVersion(thisPackageId.Version.Major, out INuGetPublishedLibraryMinorVersions minorVersions))
                    {
                        // It appears that we're trying to build something with a Major version number never
                        // seen before, but which is less than the latest version seen before.
                        // (E.g., v1.0 and v3.0 have been published, but we're now looking at v2.0.)
                        changeType = ComponentChangeType.RetrogradeMajorVersionNumber;
                    }
                    else
                    {
                        // At least one version has been published with the same major version number
                        // as the target package.
                        latestVersionWithSameMajor = minorVersions.LatestPublishedVersionWithSameMajor;
                        int thisMinorVersion = thisPackageId.Version.Minor;

                        if (thisMinorVersion > minorVersions.LatestPublishedMinorVersionWithSameMajor)
                        {
                            changeType = ComponentChangeType.MinorUpdate;
                            predecessorId = latestVersionWithSameMajor;
                        }
                        else
                        {
                            if (!minorVersions.TryGetLatestVersionWithMinorVersion(thisMinorVersion, out PackageIdentity latestPatchWithSameMajorAndMinor))
                            {
                                // It appears that we're trying to build something where the Major version has
                                // been seen before, but this particular Major.Minor combination is new, and it
                                // is lower than the latest version seen before.
                                // (E.g., v2.0 and v2.2 have been published but we're now looking at v2.1.)
                                changeType = ComponentChangeType.RetrogradeMinorVersionNumber;
                                predecessorId = latestPatchWithSameMajorAndMinor;
                            }
                            else
                            {
                                int thisPatchVersion = thisPackageId.Version.Patch;

                                if (thisPatchVersion > latestPatchWithSameMajorAndMinor.Version.Patch)
                                {
                                    changeType = ComponentChangeType.BugfixUpdate;
                                    predecessorId = latestPatchWithSameMajorAndMinor;
                                }
                                else if (thisPatchVersion < latestPatchWithSameMajorAndMinor.Version.Patch)
                                {
                                    changeType = ComponentChangeType.RetrogradePatchVersionNumber;
                                }
                                else
                                {
                                    changeType = ComponentChangeType.VersionAlreadyPublished;
                                }
                            }
                        }
                    }

                    SemanticVersionChange maximumAcceptableChange = SemanticVersionChange.None;

                    switch (changeType)
                    {
                        case ComponentChangeType.VersionAlreadyPublished:
                            Console.Error.WriteLine($"{thisPackageId} - already published with this version");
                            rulesViolated = true;
                            break;
                        case ComponentChangeType.NewComponent:
                        case ComponentChangeType.MajorUpdate:
                            maximumAcceptableChange = SemanticVersionChange.Major;
                            break;
                        case ComponentChangeType.MinorUpdate:
                            maximumAcceptableChange = SemanticVersionChange.Minor;
                            break;
                        case ComponentChangeType.BugfixUpdate:
                            break;
                        case ComponentChangeType.RetrogradeMajorVersionNumber:
                            Console.Error.WriteLine($"{thisPackageId} has major version number lower than latest ({latestPublishedMajorVersion}), and no component with this major version has previously been published");
                            rulesViolated = true;
                            break;
                        case ComponentChangeType.RetrogradeMinorVersionNumber:
                            Console.Error.WriteLine($"{thisPackageId} has minor version number lower than the latest published version with a matching major number ({latestVersionWithSameMajor})");
                            rulesViolated = true;
                            break;
                        case ComponentChangeType.RetrogradePatchVersionNumber:
                            Console.Error.WriteLine($"{thisPackageId} has patch version number lower than the latest published version with a matching major.minor number ({latestVersionWithSameMajor})");
                            rulesViolated = true;
                            break;
                        default:
                            throw new InvalidOperationException("Unknown " + nameof(ComponentChangeType));
                    }

                    if (predecessorId != null)
                    {
                        Console.WriteLine(" Comparing with " + predecessorId);

                        INuGetPackage predecessorPackage = await feed.GetPackageAsync(predecessorId).ConfigureAwait(false);
                        SemanticVersionChange packageChange = await ComparePackages(predecessorPackage, newPackage).ConfigureAwait(false);
                        predecessorPackage.Dispose();

                        if (packageChange > maximumAcceptableChange)
                        {
                            Console.Error.WriteLine($"Package '{thisPackageId.Id}' makes changes requiring a '{packageChange}' version change. The predecessor version is {predecessorId.Version}, and the target version is {thisPackageId.Version}, which is only a '{maximumAcceptableChange}' change");
                            rulesViolated = true;
                        }
                        else
                        {
                            Console.WriteLine($" Change detected: '{packageChange}', which is acceptable, because the predecessor version is {predecessorId.Version}, and the target version is {thisPackageId.Version}, which is a '{maximumAcceptableChange}' change");
                            rulesViolated = true;
                        }
                    }
                }

                newPackage.Dispose();
            }

            return !rulesViolated;
        }

        private static async Task<SemanticVersionChange> ComparePackages(INuGetPackage predecessorPackage, INuGetPackage newPackage)
        {
            SemanticVersionChange biggestChangeSeen = SemanticVersionChange.None;

            Dictionary<NuGetFramework, List<string>> predecessorItemsByFramework = await predecessorPackage.GetLibraryItemsByFrameworkAsync().ConfigureAwait(false);
            Dictionary<NuGetFramework, List<string>> newItemsByFramework = await newPackage.GetLibraryItemsByFrameworkAsync().ConfigureAwait(false);

            bool targetFrameworksRemoved = predecessorItemsByFramework.Keys.Except(newItemsByFramework.Keys).Any();

            if (targetFrameworksRemoved)
            {
                // If we've lost support for a target framework, that constitutes a major change.
                // Note: to handle this properly, we should handle cases where the framework change actually
                // broadens support. (E.g., if netstd16 has gone but we now have netstd14, then that's not
                // a breaking change.) However, we're not really expecting changes of that kind for now.
                biggestChangeSeen = SemanticVersionChange.Major;
            }

            // TODO: do we need to be intelligent about cases where the addition of a new target framework
            // could cause clients to change which of the package's support frameworks they use? E.g., a
            // package offering netstandard1.6 could conceivably also offer a net462 (or whatever) in addition
            // to the netstd one in some later version. .NET Core clients would continue to get the netstd one,
            // but NETFX ones would prefer the net462 version. If this were done in a minor release, we should
            // really verify that the net462 version doesn't introduce breaking changes relative to the
            // netstandard1.6 one.
            // However, this is a relatively unusual scenario. I would not expect us to be changing the set of
            // supported target frameworks within a single major version.
            foreach ((NuGetFramework framework, List<string> predecessorItems) in predecessorItemsByFramework.Where(x => newItemsByFramework.ContainsKey(x.Key)))
            {
                List<string> targetItems = newItemsByFramework[framework];
                bool libItemsRemoved = predecessorItems.Except(targetItems).Any();

                if (libItemsRemoved)
                {
                    // With this target framework, there's a library item that was present in the predecessor
                    // that is not present in the target version. If an entire DLL goes missing, that's a
                    // breaking change.
                    biggestChangeSeen = SemanticVersionChange.Major;
                }

                bool libItemsAdded = targetItems.Except(predecessorItems).Any();

                if (libItemsAdded)
                {
                    // With this target framework, there's a library item in the target version that was not
                    // present in the predecessor. This won't break existing clients, but it means that
                    // clients built against this version can't go back to an older one.
                    biggestChangeSeen = biggestChangeSeen.AtLeast(SemanticVersionChange.Minor);
                }

                string workingFolder = Path.Combine(Path.GetTempPath(), @"endjin\nupkgversion\" + Guid.NewGuid());
                string predecessorFolder = workingFolder + @"\before";
                string targetFolder = workingFolder + @"\after";

                try
                {
                    Directory.CreateDirectory(predecessorFolder);
                    Directory.CreateDirectory(targetFolder);
                    var allItems = predecessorItems.Union(targetItems).ToList();

                    await predecessorPackage.CopyFilesAsync(predecessorFolder, allItems).ConfigureAwait(false);
                    await newPackage.CopyFilesAsync(targetFolder, allItems).ConfigureAwait(false);

                    foreach (string libItem in predecessorItems)
                    {
                        Console.WriteLine(" Comparing " + libItem);
                        string predecessorPath = predecessorFolder + "\\" + libItem;
                        string targetPath = targetFolder + "\\" + libItem;

                        SemanticVersionChange changeForThisItem = CheckAssembliesForSemverChanges(predecessorPath, targetPath);
                        Console.WriteLine("  " + changeForThisItem);
                        biggestChangeSeen = biggestChangeSeen.AtLeast(changeForThisItem);
                    }
                }
                finally
                {
                    if (Directory.Exists(workingFolder))
                    {
                        Directory.Delete(workingFolder, true);
                    }
                }
            }

            return biggestChangeSeen;
        }

        private static SemanticVersionChange CheckAssembliesForSemverChanges(string predecessorPath, string targetPath)
        {
            LibraryChanges changes = LibraryComparison.DetectChanges(predecessorPath, targetPath);
            return LibrarySemanticVersionChangeAnalyzer.GetMinimumAcceptableChange(changes);
        }
    }
}

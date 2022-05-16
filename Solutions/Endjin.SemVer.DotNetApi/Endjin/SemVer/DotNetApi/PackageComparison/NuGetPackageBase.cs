// <copyright file="NuGetPackageBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using NuGet.Common;
    using NuGet.Frameworks;
    using NuGet.Packaging;
    using NuGet.Packaging.Core;

    /// <summary>
    /// Common functionality for local- and feed-based <see cref="INuGetPackage"/> implementations.
    /// </summary>
    internal abstract class NuGetPackageBase : INuGetPackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageBase"/> class.
        /// </summary>
        /// <param name="identity">The <see cref="Identity"/>.</param>
        /// <param name="nuGetLogger">NuGet library logger.</param>
        protected NuGetPackageBase(PackageIdentity identity, ILogger nuGetLogger)
        {
            this.Identity = identity;
            this.NuGetLogger = nuGetLogger;
        }

        /// <summary>
        /// Gets the NuGet package's identity.
        /// </summary>
        public PackageIdentity Identity { get; }

        /// <summary>
        /// Gets the NuGet logger.
        /// </summary>
        public ILogger NuGetLogger { get; }

        /// <inheritdoc/>
        public async Task CopyFilesAsync(string folder, List<string> allItems)
        {
            PackageReaderBase packageReader = this.GetPackageReader();

            await packageReader.CopyFilesAsync(
                folder,
                allItems,
                CopyCallback,
                this.NuGetLogger,
                CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<NuGetFramework, List<string>>> GetLibraryItemsByFrameworkAsync()
        {
            PackageReaderBase packageReader = this.GetPackageReader();
            IEnumerable<FrameworkSpecificGroup> libItems = await packageReader.GetLibItemsAsync(CancellationToken.None).ConfigureAwait(false);
            return libItems.ToDictionary(x => x.TargetFramework, x => x.Items.ToList());
        }

        /// <inheritdoc/>
        public abstract void Dispose();

        /// <summary>
        /// Helper for copying files out of packages.
        /// </summary>
        /// <param name="sourceFile">
        /// The source file path. Depending on where the package is stored, this may or may not
        /// exist.
        /// </param>
        /// <param name="targetPath">
        /// The location to which to copy the file.
        /// </param>
        /// <param name="fileStream">
        /// The contents of the file. When <c>sourceFile</c> doesn't exist, we can use this
        /// instead.
        /// </param>
        /// <returns>The location to which the file was copied.</returns>
        protected static string CopyCallback(string sourceFile, string targetPath, Stream fileStream)
        {
            if (File.Exists(sourceFile))
            {
                File.Copy(sourceFile, targetPath);
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

                using (FileStream output = File.Create(targetPath))
                {
                    fileStream.CopyTo(output);
                }
            }

            return targetPath;
        }

        /// <summary>
        /// Gets the NuGet PackageReader.
        /// </summary>
        /// <returns>The package reader.</returns>
        protected abstract PackageReaderBase GetPackageReader();
    }
}
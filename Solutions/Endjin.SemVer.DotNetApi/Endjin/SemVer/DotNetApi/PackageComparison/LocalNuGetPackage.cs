// <copyright file="LocalNuGetPackage.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.PackageComparison
{
    using NuGet.Common;
    using NuGet.Packaging;
    using NuGet.Protocol;

    /// <summary>
    /// Implements <see cref="INuGetPackage"/> for packages in the local filesystem.
    /// </summary>
    internal class LocalNuGetPackage : NuGetPackageBase
    {
        private readonly LocalPackageInfo packageInfo;
        private PackageReaderBase reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalNuGetPackage"/> class.
        /// </summary>
        /// <param name="packageInfo">The package information.</param>
        /// <param name="nuGetLogger">The NuGet logger.</param>
        public LocalNuGetPackage(LocalPackageInfo packageInfo, ILogger nuGetLogger)
            : base(packageInfo.Identity, nuGetLogger)
        {
            this.packageInfo = packageInfo;
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            this.reader?.Dispose();
        }

        /// <inheritdoc/>
        protected override PackageReaderBase GetPackageReader() => this.reader = this.reader ?? this.packageInfo.GetReader();
    }
}
// <copyright file="ComponentChangeType.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    /// <summary>
    /// Describes the change between a new version of a package and its predecessor based on their
    /// version numbers.
    /// </summary>
    public enum ComponentChangeType
    {
        /// <summary>
        /// The 'new' package has the same version number as one already published.
        /// </summary>
        VersionAlreadyPublished,

        /// <summary>
        /// There are no previously published versions of this package.
        /// </summary>
        NewComponent,

        /// <summary>
        /// The new package has a higher major version number than any previously published
        /// version of this package.
        /// </summary>
        MajorUpdate,

        /// <summary>
        /// The new package shares a major version number with at least one previously published
        /// package, but has a higher minor version than any previously published package that
        /// has the same major version number.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In addition to the most straightforward scenario in which the new package is newer than
        /// any previously published version, this will also be reported in cases where newer major
        /// versions exist, but the new package is the newest minor version of a particular major
        /// version.
        /// </para>
        /// <para>
        /// For example, suppose these have been published: v1.0.0, v1.1.0, v2.0.0. If the new
        /// package is v1.2.0, that is a lower version number than the latest published one. But
        /// this can be perfectly valid: older major versions may still be supported, and we might
        /// want to release a new v1.x library despite the latest, greatest now being v2.x.
        /// </para>
        /// </remarks>
        MinorUpdate,

        /// <summary>
        /// The new package version shares a major.minor version number with at least one
        /// previously published package, but has a higher patch number than any previously
        /// published package that shares the major.minor version number.
        /// </summary>
        /// <remarks>
        /// <para>
        /// As with <see cref="MinorUpdate"/>, this can include cases where higher version numbers
        /// exist than the 'new' package. E.g., the latest published may be v2.2. But if we had
        /// also previously published v1.1.0 and v1.2.0, it would be perfectly valid to publish
        /// new packages with either v1.1.1, or v1.2.1. Either of those would be a bugfix update
        /// to their respective v1.1.0 and v1.2.0 predecessors.
        /// </para>
        /// </remarks>
        BugfixUpdate,

        /// <summary>
        /// The new package version has a major version number that has never been published before
        /// and yet is lower than the highest major version number of any previously published
        /// version.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is typically an error. E.g., we've published v4.0, v4.1, and v5.0, and now we're
        /// trying to publish a 'new' library with v1.0. That's a step backwards, and nobody is
        /// likely to pick the library up, since anyone already using the library will already be
        /// on v4.x or v5.x, and anyone starting new development will most likely pick up v5.0.
        /// </para>
        /// <para>
        /// This is not technically illegal, but it's almost always indicative of a mistake.
        /// </para>
        /// </remarks>
        RetrogradeMajorVersionNumber,

        /// <summary>
        /// The new package version shares a major version number with one or more existing
        /// published versions, but has a minor version number that is different from every other
        /// minor version number of previously published versions sharing a major version number,
        /// and also lower than the higest such minor version numbers.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For example, this would occur if v1.0, v1.2, and v1.3 had all previously been published
        /// and the new package has version v1.1.
        /// </para>
        /// <para>
        /// This is not technically illegal, but it's almost always indicative of a mistake.
        /// </para>
        /// </remarks>
        RetrogradeMinorVersionNumber,

        /// <summary>
        /// The new package version shares a major.minor version with one or more existing
        /// published versions, but its patch number is different from every other published
        /// version with matching major.minor, and also lower than one or more of the patch numbers
        /// of those matching versions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For example, this would occur if v1.0.23, v1.0.82, and v1.0.112 had all previously been
        /// published and the new package has version v1.0.0.
        /// </para>
        /// <para>
        /// This is not technically illegal, but it's almost always indicative of a mistake.
        /// </para>
        /// </remarks>
        RetrogradePatchVersionNumber,
    }
}

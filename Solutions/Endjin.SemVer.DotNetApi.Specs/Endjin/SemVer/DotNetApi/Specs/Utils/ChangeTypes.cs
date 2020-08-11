// <copyright file="ChangeTypes.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Specs.Utils
{
    using System;

    /// <summary>
    /// Describes the changes made to the public surface of a library.
    /// </summary>
    [Flags]
    public enum ChangeTypes
    {
        /// <summary>
        /// No changes.
        /// </summary>
        None = 0b0,

        /// <summary>
        /// At least one public field added.
        /// </summary>
        AddPublicField = 0b1,

        /// <summary>
        /// At least one public constructor added.
        /// </summary>
        AddPublicConstructor = 0b10,

        /// <summary>
        /// At least one public method added.
        /// </summary>
        AddPublicMethod = 0b100,

        /// <summary>
        /// At least one public property added.
        /// </summary>
        AddPublicProperty = 0b1000,

        /// <summary>
        /// At least one public type method added.
        /// </summary>
        AddPublicType = 0b1_0000,

        /// <summary>
        /// At least one public field removed.
        /// </summary>
        RemovePublicField = 0b10_0000,

        /// <summary>
        /// At least one public constructor removed.
        /// </summary>
        RemovePublicConstructor = 0b100_0000,

        /// <summary>
        /// At least one public method removed.
        /// </summary>
        RemovePublicMethod = 0b1000_0000,

        /// <summary>
        /// At least one public property removed.
        /// </summary>
        RemovePublicProperty = 0b1_0000_0000,

        /// <summary>
        /// At least one public type method removed.
        /// </summary>
        RemovePublicType = 0b10_0000_0000,
    }
}

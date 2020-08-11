// <copyright file="TestMethodSignatureTypes.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Specs.Utils
{
    public enum TestMethodSignatureTypes
    {
        /// <summary>
        /// No return type, and no parameters
        /// </summary>
        VoidNoParams,

        /// <summary>
        /// No return type, string parameter.
        /// </summary>
        VoidReturnStringParams,

        /// <summary>
        /// No return type, string and int parameters.
        /// </summary>
        VoidReturnStringAndIntParams,

        /// <summary>
        /// Return type of string, and no parameters.
        /// </summary>
        StringReturnNoParams,

        /// <summary>
        /// Return type of string, string and int parameters.
        /// </summary>
        StringReturnStringAndIntParams,
    }
}

// <copyright file="ChangeTypesExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Specs.Utils
{
    public static class ChangeTypesExtensions
    {
        public static bool Includes(this ChangeTypes changeTypes, ChangeTypes changeType) => (changeTypes & changeType) != ChangeTypes.None;
    }
}

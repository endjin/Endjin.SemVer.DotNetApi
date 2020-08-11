// <copyright file="GlobalSuppressions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "StyleCop.CSharp.SpacingRules",
    "SA1025:Code should not contain multiple whitespace in a row",
    Justification = "With a flags enumerations, vertical alignment of the constant values makes it easier to see how the bits are used",
    Scope = "type",
    Target = "~T:Endjin.ApiCompare.LibraryChangeTestSource.ChangeTypes")]

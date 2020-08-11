// <copyright file="LibrarySemanticVersionChangeAnalyzer.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    using System.Linq;

    /// <summary>
    /// Analyzes <see cref="LibraryChanges"/> to determine the minimum acceptable version number
    /// change according to Semantic Versioning rules.
    /// </summary>
    public static class LibrarySemanticVersionChangeAnalyzer
    {
        /// <summary>
        /// Calculate the minimum acceptable version number change according to Semantic Versioning
        /// rules given some changes that have been made to a library.
        /// </summary>
        /// <param name="libraryChanges">The changes made to the library.</param>
        /// <returns>The minimum version number change acceptable in Semantic Versioning.</returns>
        public static SemanticVersionChange GetMinimumAcceptableChange(LibraryChanges libraryChanges)
        {
            bool typesRemoved = libraryChanges.TypesRemoved.Count > 0;
            bool constructorsRemoved = libraryChanges.TypesChanged.Any(t => t.ConstructorsRemoved.Count > 0);
            bool methodsRemoved = libraryChanges.TypesChanged.Any(t => t.MethodsRemoved.Count > 0);
            bool propertiesRemoved = libraryChanges.TypesChanged.Any(t => t.PropertiesRemoved.Count > 0);
            bool fieldsRemoved = libraryChanges.TypesChanged.Any(t => t.FieldsRemoved.Count > 0);

            if (typesRemoved || constructorsRemoved || methodsRemoved || propertiesRemoved || fieldsRemoved)
            {
                return SemanticVersionChange.Major;
            }

            bool typesAdded = libraryChanges.TypesAdded.Count > 0;
            bool constructorsAdded = libraryChanges.TypesChanged.Any(t => t.ConstructorsAdded.Count > 0);
            bool methodsAdded = libraryChanges.TypesChanged.Any(t => t.MethodsAdded.Count > 0);
            bool propertiesAdded = libraryChanges.TypesChanged.Any(t => t.PropertiesAdded.Count > 0);
            bool fieldsAdded = libraryChanges.TypesChanged.Any(t => t.FieldsAdded.Count > 0);

            return typesAdded || constructorsAdded || methodsAdded || propertiesAdded || fieldsAdded
                ? SemanticVersionChange.Minor
                : SemanticVersionChange.None;
        }
    }
}

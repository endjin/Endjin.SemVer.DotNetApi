// <copyright file="LibraryChanges.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes the changes between two versions of a library.
    /// </summary>
    public class LibraryChanges
    {
        /// <summary>
        /// Create a <see cref="LibraryChanges"/>.
        /// </summary>
        /// <param name="typesAdded">The <see cref="TypesAdded"/>.</param>
        /// <param name="typesChanged">The <see cref="TypesChanged"/>.</param>
        /// <param name="typesRemoved">The <see cref="TypesRemoved"/>.</param>
        public LibraryChanges(IReadOnlyList<LibraryType> typesAdded, IReadOnlyList<LibraryType> typesChanged, IReadOnlyList<LibraryType> typesRemoved)
        {
            foreach (LibraryType t in typesAdded)
            {
                if (t.ConstructorsRemoved.Count != 0)
                {
                    throw new ArgumentException($"Added types cannot remove constructors, but {t} removes {t.ConstructorsRemoved.Count} constructors", nameof(typesAdded));
                }

                if (t.MethodsRemoved.Count != 0)
                {
                    throw new ArgumentException($"Added types cannot remove members, but {t} removes {t.MethodsRemoved.Count} methods", nameof(typesAdded));
                }

                if (t.PropertiesRemoved.Count != 0)
                {
                    throw new ArgumentException($"Added types cannot remove members, but {t} removes {t.PropertiesRemoved.Count} properties", nameof(typesAdded));
                }

                if (t.EventsRemoved.Count != 0)
                {
                    throw new ArgumentException($"Added types cannot remove members, but {t} removes {t.EventsRemoved.Count} events", nameof(typesAdded));
                }

                if (t.FieldsRemoved.Count != 0)
                {
                    throw new ArgumentException($"Added types cannot remove members, but {t} removes {t.FieldsRemoved.Count} fields", nameof(typesAdded));
                }
            }

            foreach (LibraryType t in typesChanged)
            {
                if (t.ConstructorsAdded.Count == 0 &&
                    t.ConstructorsRemoved.Count == 0 &&
                    t.MethodsRemoved.Count == 0 &&
                    t.MethodsAdded.Count == 0 &&
                    t.PropertiesAdded.Count == 0 &&
                    t.PropertiesRemoved.Count == 0 &&
                    t.EventsAdded.Count == 0 &&
                    t.EventsRemoved.Count == 0 &&
                    t.FieldsAdded.Count == 0 &&
                    t.FieldsRemoved.Count == 0)
                {
                    throw new ArgumentException($"Changed types must add or remove at least one member, but {t} has no changes", nameof(typesChanged));
                }
            }

            foreach (LibraryType t in typesRemoved)
            {
                if (t.ConstructorsAdded.Count != 0)
                {
                    throw new ArgumentException($"Removed types cannot add members, but {t} adds {t.ConstructorsAdded.Count} constructors", nameof(typesRemoved));
                }

                if (t.MethodsAdded.Count != 0)
                {
                    throw new ArgumentException($"Removed types cannot add members, but {t} adds {t.MethodsAdded.Count} methods", nameof(typesRemoved));
                }

                if (t.PropertiesAdded.Count != 0)
                {
                    throw new ArgumentException($"Removed types cannot add members, but {t} adds {t.PropertiesAdded.Count} properties", nameof(typesRemoved));
                }

                if (t.EventsAdded.Count != 0)
                {
                    throw new ArgumentException($"Removed types cannot add members, but {t} adds {t.EventsAdded.Count} events", nameof(typesRemoved));
                }

                if (t.FieldsAdded.Count != 0)
                {
                    throw new ArgumentException($"Removed types cannot add members, but {t} adds {t.FieldsAdded.Count} fields", nameof(typesRemoved));
                }
            }

            this.TypesAdded = typesAdded;
            this.TypesChanged = typesChanged;
            this.TypesRemoved = typesRemoved;
        }

        /// <summary>
        /// Gets a list of types that have been added in the newer version of the library.
        /// </summary>
        public IReadOnlyList<LibraryType> TypesAdded { get; }

        /// <summary>
        /// Gets a list of types that have changed in some way between the newer and older versions
        /// of the library.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The types listed here are present in both versions, but something about them has
        /// changed, e.g., members have been added or removed.
        /// </para>
        /// </remarks>
        public IReadOnlyList<LibraryType> TypesChanged { get; }

        /// <summary>
        /// Gets a list of types that have been removed in the newer version of the library.
        /// </summary>
        public IReadOnlyList<LibraryType> TypesRemoved { get; }
    }
}
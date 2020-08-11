// <copyright file="LibraryEvent.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Versioning
{
    /// <summary>
    /// Describes an event which has added, removed, or changed in some way.
    /// </summary>
    public class LibraryEvent
    {
        /// <summary>
        /// Creates a <see cref="LibraryEvent"/>.
        /// </summary>
        /// <param name="name">The event's <see cref="Name"/>.</param>
        /// <param name="type">The events's <see cref="Type"/>.</param>
        public LibraryEvent(
            string name,
            LibraryTypeReference type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets the event's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the event's type.
        /// </summary>
        public LibraryTypeReference Type { get; }
    }
}

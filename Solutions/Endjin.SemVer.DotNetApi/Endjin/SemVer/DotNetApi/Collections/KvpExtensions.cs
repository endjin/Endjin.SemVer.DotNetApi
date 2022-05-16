// <copyright file="KvpExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.SemVer.DotNetApi.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for <see cref="KeyValuePair{TKey, TValue}"/>.
    /// </summary>
    internal static class KvpExtensions
    {
        /// <summary>
        /// Enable deconstruction of <see cref="KeyValuePair{TKey, TValue}"/> on .NET FX.
        /// </summary>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="kvp">The key value pair to deconstruct.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <remarks>
        /// <para>
        /// This enables us to do this sort of thing:
        /// </para>
        /// <code>
        /// foreach ((int key, string value) in someDictionary) ...
        /// </code>
        /// <para>
        /// Irritatingly, although .NET Standard 2.0 supports this deconstruction (meaning it will
        /// actually work just fine at runtime in .NET FX) it's not available if your compilation
        /// target is .NET FX. This method makes it available.
        /// </para>
        /// </remarks>
        public static void Deconstruct<TKey, TValue>(
            this KeyValuePair<TKey, TValue> kvp,
            out TKey key,
            out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}
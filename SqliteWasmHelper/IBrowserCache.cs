// <copyright file="IBrowserCache.cs" company="Jeremy Likness">
// Copyright (c) Jeremy Likness. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;

namespace SqliteWasmHelper
{
    /// <summary>
    /// Wrapper for JavaScript module functions that interact with the cache.
    /// </summary>
    public interface IBrowserCache
    {
        /// <summary>
        /// Calls the code to check save to/restore from cache.
        /// </summary>
        /// <param name="filename">The name of the file to process.</param>
        /// <returns>Either -1 (no cache), 0 (restored), or 1 (cached).</returns>
        Task<int> SyncDbWithCacheAsync(string filename);

        /// <summary>
        /// Creates an anchor tag to download the database and injects it into the parent.
        /// </summary>
        /// <param name="parent">The host for the tag.</param>
        /// <param name="filename">The database filename.</param>
        /// <returns>A value indicating whether the operation was successful.</returns>
        Task<bool> GenerateDownloadLinkAsync(ElementReference parent, string filename);
    }
}

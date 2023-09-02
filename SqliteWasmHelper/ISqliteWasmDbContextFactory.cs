// <copyright file="ISqliteWasmDbContextFactory.cs" company="Jeremy Likness">
// Copyright (c) Jeremy Likness. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;

namespace SqliteWasmHelper
{
    /// <summary>
    /// Interface for custom factory.
    /// </summary>
    /// <typeparam name="TContext">The <see cref="DbContext"/> to create.</typeparam>
    public interface ISqliteWasmDbContextFactory<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Gets the <see cref="DbContext"/>.
        /// </summary>
        /// <returns>The new context.</returns>
        Task<TContext> CreateDbContextAsync();

        /// <summary>
        /// Calls the code to restore the database from a given ArrayBuffer.
        /// </summary>
        /// <param name="arrayBuffer">The ArrayBuffer containing the database file.</param>
        /// <returns>0 if successful, -1 otherwise.</returns>
        Task<int> ManualRestore(byte[] arrayBuffer);
    }
}

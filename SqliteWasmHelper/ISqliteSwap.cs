// <copyright file="ISqliteSwap.cs" company="Jeremy Likness">
// Copyright (c) Jeremy Likness. All rights reserved.
// </copyright>

namespace SqliteWasmHelper
{
    /// <summary>
    /// Encapsulates backup functionality for SQLite.
    /// </summary>
    public interface ISqliteSwap
    {
        /// <summary>
        /// Perform a backup operation (or restore) between two databases.
        /// </summary>
        /// <param name="srcFilename">Name of the source filename of the db.</param>
        /// <param name="destFilename">Name of the destination filename of the db.</param>
        void DoSwap(string srcFilename, string destFilename);
    }
}

// <copyright file="SqliteSwap.cs" company="Jeremy Likness">
// Copyright (c) Jeremy Likness. All rights reserved.
// </copyright>

using Microsoft.Data.Sqlite;

namespace SqliteWasmHelper
{
    /// <summary>
    /// Performs the backup or restore. Override to inject your own
    /// functionality, such as sending the file to the server.
    /// </summary>
    public class SqliteSwap : ISqliteSwap
    {
        /// <summary>
        /// Performs the swap between live database and backup.
        /// </summary>
        /// <param name="srcFilename">Name of the source db file.</param>
        /// <param name="destFilename">Name of the target db file.</param>
        public void DoSwap(string srcFilename, string destFilename)
        {
            using var src = new SqliteConnection($"Data Source={srcFilename}");
            using var tgt = new SqliteConnection($"Data Source={destFilename}");

            src.Open();
            tgt.Open();

            src.BackupDatabase(tgt);

            tgt.Close();
            src.Close();
        }
    }
}

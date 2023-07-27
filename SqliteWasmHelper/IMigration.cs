// <copyright file="IMigration.cs" company="Jeremy Likness">
// Copyright (c) Jeremy Likness. All rights reserved.
// </copyright>

namespace SqliteWasmHelper
{
    /// <summary>
    /// Determines whether to use the migration strategy or ensure created.
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// Reads the prefered entity framework database creation strategy.
        /// </summary>
        /// <returns>True to use migration. False to use ensure created.</returns>
        bool UseMigration();
    }
}

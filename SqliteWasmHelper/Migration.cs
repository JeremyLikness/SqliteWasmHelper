// <copyright file="Migration.cs" company="Jeremy Likness">
// Copyright (c) Jeremy Likness. All rights reserved.
// </copyright>

namespace SqliteWasmHelper
{
    /// <summary>
    /// Class for determining migration method.
    /// </summary>
    public class Migration : IMigration
    {
        private readonly bool useMigration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration"/> class.
        /// </summary>
        /// <param name="useMigration">True for using entity framework migrations. False for ensure created.</param>
        public Migration(bool useMigration = false)
        {
            this.useMigration = useMigration;
        }

        /// <summary>
        /// True for using entity framework migrations. False for ensure created.
        /// </summary>
        /// <returns>Boolean true for using entity framework migrations. False for ensure created.</returns>
        public bool UseMigration() => useMigration;
    }
}

using Microsoft.Data.Sqlite;
using SqliteWasmHelper;
using System;
using Xunit;

namespace SqliteWasmTests
{
    public class SqliteSwapTests
    {
        [Fact]
        public void SqliteSwapBackupsUpFromOneDatabaseToAnother()
        {
            // arrange
            var db1 = Guid.NewGuid().ToString().Replace("-", string.Empty) +
                ".sqlite3";
            var db2 = Guid.NewGuid().ToString().Replace("-", string.Empty) +
                ".sqlite3";
            var ds1 = $"Data Source={db1}";
            var ds2 = $"Data Source={db2}";

            using (var conn = new SqliteConnection(ds1))
            {
                conn.Open();
                string sql = "create table names (name varchar(50))";

                var command = new SqliteCommand(sql, conn);
                command.ExecuteNonQuery();

                sql = $"insert into names (name) values ('{db1}')";

                command = new SqliteCommand(sql, conn);
                command.ExecuteNonQuery();

                conn.Close();
            }
            var swap = new SqliteSwap();

            // act
            swap.DoSwap(db1, db2);

            string actual = string.Empty;

            // assert
            using (var conn2 = new SqliteConnection(ds2))
            {
                conn2.Open();

                string sql = "select name from names";

                var command = new SqliteCommand(sql, conn2);
                using var reader = command.ExecuteReader();
                actual = reader.Read()
                    ? reader!["name"].ToString()!
                    : string.Empty;
            }

            Assert.Equal(db1.Trim(), actual.Trim());
        }
    }
}

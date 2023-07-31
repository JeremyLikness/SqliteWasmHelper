using SqliteWasmHelper;

namespace SqliteWasmTests.TestHelpers
{
    public class MockMigration : IMigration
    {
        public bool UseMigration() => false;
    }
}

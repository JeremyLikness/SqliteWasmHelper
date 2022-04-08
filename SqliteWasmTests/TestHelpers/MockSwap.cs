using SqliteWasmHelper;

namespace SqliteWasmTests.TestHelpers
{
    public class MockSwap : ISqliteSwap
    {
        public string? Source { get; private set; }

        public string? Target { get; private set; }
        public void DoSwap(string srcFilename, string destFilename)
        {
            Source = srcFilename;
            Target = destFilename;
        }
    }
}

using System;
using System.Linq;

namespace SqliteWasmTests.TestHelpers
{
    public class TestThing
    {
        private static readonly string[] colors = Enum.GetValues<ConsoleColor>()
            .Select(c => c.ToString()).ToArray();
        private static readonly string[] keys = Enum.GetValues<ConsoleKey>()
            .Select(c => c.ToString()).ToArray();        
        private static readonly Random random = new();
        private static string RandomName => 
            $"{colors[random.Next(colors.Length)]} {keys[random.Next(keys.Length)]}";

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = RandomName;
    }
}

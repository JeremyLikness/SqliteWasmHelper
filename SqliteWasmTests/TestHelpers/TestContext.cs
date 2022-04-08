using Microsoft.EntityFrameworkCore;

namespace SqliteWasmTests.TestHelpers
{
    public class TestContext : DbContext
    {
        public DbContextOptions<TestContext> options { get; private set; }

        public TestContext(DbContextOptions<TestContext> opts) : base(opts) =>
            options = opts;
        
        public DbSet<TestThing> Things { get; set; } = null!;
    }
}

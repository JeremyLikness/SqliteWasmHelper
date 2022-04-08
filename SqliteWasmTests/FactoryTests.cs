using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqliteWasmHelper;
using SqliteWasmTests.TestHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SqliteWasmTests
{
    public class FactoryTests
    {
        private readonly MockBrowserCache mockBrowserCache;
        private IDbContextFactory<TestContext>? dbContextFactory;
        private readonly MockSwap swap;
        private readonly string filename;
        private readonly string altFilename;

        private ISqliteWasmDbContextFactory<TestContext> CreateFactory(
            string? ds = null)
        {
            var sc = new TestServiceCollection();
            sc.AddDbContextFactory<TestContext>(
                opts => opts.UseSqlite(ds ?? $"Data Source={filename}"));
            var sp = sc.BuildServiceProvider();
            dbContextFactory = sp.CreateScope()
                .ServiceProvider
                .GetRequiredService<IDbContextFactory<TestContext>>();

            SqliteWasmDbContextFactory<TestContext>.Reset();
            var factory = new SqliteWasmDbContextFactory<TestContext>(
                dbContextFactory,
                mockBrowserCache,
                swap);
            return factory;
        }

        public FactoryTests()
        {
            mockBrowserCache = new MockBrowserCache();
            filename = Guid.NewGuid().ToString();
            altFilename = Guid.NewGuid().ToString();
            swap = new MockSwap();
        }

        [Fact]
        public void FactoryCallsSyncDbOnConstruction()
        {
            // arrange
            mockBrowserCache.SyncDbResult = 0;
            
            // act
            var _ = CreateFactory();

            // assert
            Assert.True(mockBrowserCache.SyncDbCallCount == 1);
            Assert.NotEmpty(mockBrowserCache.Filenames);
            Assert.Contains(                
                $"{filename}_bak",
                mockBrowserCache.Filenames);
        }

        [Fact]
        public void FactoryDoesNotRestoreWhenBadResultCode()
        {
            // arrange
            mockBrowserCache.SyncDbResult = -1;

            // act
            var _ = CreateFactory();

            // assert
            Assert.True(string.IsNullOrEmpty(swap.Source));
            Assert.True(string.IsNullOrEmpty(swap.Target));
        }

        [Fact]
        public void FactoryRestoresWhenOkResultCode()
        {
            // arrange
            mockBrowserCache.SyncDbResult = 0;

            // act
            var _ = CreateFactory();

            // assert
            Assert.Equal($"{filename}_bak", swap.Source);
            Assert.Equal($"{filename}", swap.Target);
        }

        [Theory]
        [InlineData("Data Source=one.db; Version=3;", "one.db")]
        [InlineData("Data Source=:memory:; Version=3;", ":memory:")]
        [InlineData("Filename=two.db; Version=3; Compress=True", "two.db")]
        public void FactoryParsesFilenameFromDataSource(
            string dataSource,
            string filename)
        {
            // arrange
            mockBrowserCache.SyncDbResult = -1;

            // act
            var _ = CreateFactory(dataSource);

            // assert
            Assert.Contains($"{filename}_bak", mockBrowserCache.Filenames);
        }       
    }
}

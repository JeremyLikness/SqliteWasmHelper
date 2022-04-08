using Microsoft.AspNetCore.Components;
using SqliteWasmHelper;
using SqliteWasmTests.TestHelpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SqliteWasmTests
{
    public class BrowserCacheTests
    {
        private MockJsModule module;
        private MockJsRuntime runtime;

        private IBrowserCache NewBrowserCache() =>
            new BrowserCache(runtime);        

        public BrowserCacheTests()
        {
            module = new MockJsModule();
            runtime = new MockJsRuntime
            {
                JSObjectReference = module
            };
        }

        [Fact]
        public async void CtorLoadsModule()
        {
            // arrange
            var cache = NewBrowserCache();

            // act - module is lazy-loaded
            await cache.GenerateDownloadLinkAsync(new ElementReference(), string.Empty);

            // assert
            Assert.Equal("import", module.ModuleIdentifier);
            Assert.NotNull(module.ModuleLocation);
            Assert.Contains(
                nameof(BrowserCache), 
                module.ModuleLocation,
                StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public async Task ModuleIsProperlyDisposed()
        {
            // arrange
            var newBrowser = NewBrowserCache();
            await newBrowser.GenerateDownloadLinkAsync(new ElementReference(), string.Empty);

            // act
            await ((IAsyncDisposable)newBrowser).DisposeAsync();

            // assert
            Assert.True(module.Disposed);
        }

        [Fact]
        public async Task SyncCallsSynchronizeDb()
        {
            //arrange
            var filename = Guid.NewGuid().ToString();
            var newBrowser = NewBrowserCache();

            // act
            var _ = await newBrowser.SyncDbWithCacheAsync(filename);

            // assert
            Assert.Equal("synchronizeDbWithCache", module.Identifier);
            Assert.NotNull(module.Args);
            Assert.Contains(filename, module.Args);
        }

        [Fact]
        public async Task GetDownLoadLinkCallsGenerateDownloadLinkWithParentElementAndFilename()
        {
            //arrange
            var parent = new ElementReference();
            var filename = Guid.NewGuid().ToString();
            var newBrowser = NewBrowserCache();

            // act
            var _ = await newBrowser.GenerateDownloadLinkAsync(parent, filename);

            // assert
            Assert.Equal("generateDownloadLink", module.Identifier);
            Assert.NotNull(module.Args);
            Assert.Contains(filename, module.Args);
            Assert.Contains(parent, module.Args);
        }
    }
}

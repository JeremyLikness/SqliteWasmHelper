using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SqliteWasmHelper
{
    /// <summary>
    /// Wrapper for JavaScript code to sychronize the database.
    /// </summary>
    public sealed class BrowserCache : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserCache"/> class.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance.</param>
        public BrowserCache(IJSRuntime jsRuntime)
        {
            moduleTask = new (() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/SqliteWasmHelper/browserCache.js").AsTask() !);
        }


        public async Task<int> SyncDbWithCacheAsync(string filename)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<int>("synchronizeDbWithCache", filename);
        }

        public async Task<bool> GenerateDownloadLinkAsync(ElementReference parent, string filename)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<bool>("generateDownloadLink", parent, filename);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}

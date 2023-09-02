using Microsoft.AspNetCore.Components;
using SqliteWasmHelper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqliteWasmTests.TestHelpers
{
    public class MockBrowserCache : IBrowserCache
    {
        private DelayedTrigger<int>? delayedTrigger;

        public ElementReference? ElementRef { get; private set; }
        public Queue<string> Filenames { get; private set; }
            = new Queue<string>();

        public bool DelayResults { get; set; }

        public int GenerateDLinkCallCount { get; private set; }

        public int SyncDbCallCount { get; private set; }

        public bool GenerateDLinkResult { get; set; }
        public int SyncDbResult { get; set; }               

        public void Trigger()
        {
            if (delayedTrigger != null)
            {
                delayedTrigger.Trigger(SyncDbResult);
            }
            delayedTrigger = null;
        }

        public Task<bool> GenerateDownloadLinkAsync(
            ElementReference parent, 
            string filename)
        {
            ElementRef = parent;
            Filenames.Enqueue(filename);
            GenerateDLinkCallCount++;
            return Task.FromResult(GenerateDLinkResult);
        }

        public Task<int> SyncDbWithCacheAsync(string filename)
        {
            Filenames.Enqueue(filename);
            SyncDbCallCount++;
            if (DelayResults)
            {
                delayedTrigger = new DelayedTrigger<int>();
                return delayedTrigger.DelayUntilTriggeredAsync();
            }
            return Task.FromResult(SyncDbResult);
        }

        public Task<int> ManualRestore(byte[] arrayBuffer, string filename)
        {
            throw new System.NotImplementedException();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MvpSummit.Domain;

namespace MvpSummitWasm.Data
{
    public class SynchronizedSummitDbContextFactory<TContext> where TContext: DbContext
    {
        private readonly IDbContextFactory<TContext> dbContextFactory;
        private readonly BrowserCache cache;
        private Task<int>? lastTask = null;
        private int lastStatus = -2;
        private bool init = false;
        private string backupName = backup;

        public const string backup = $"{dbFilename}_bak";
        public const string dbFilename = "todos.sqlite3";

        public SynchronizedSummitDbContextFactory(
            IJSRuntime js,
            IDbContextFactory<SummitTaskContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
            lastTask = SynchronizeAsync();
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<SummitTaskContext> CreateSummitContextAsync()
        {
            await CheckForPendingTasksAsync();
            var ctx = await dbContextFactory.CreateDbContextAsync();

            if (!init)
            {
                Console.WriteLine($"Last status: {lastStatus}");
                await ctx.Database.EnsureCreatedAsync();
                init = true;
            }

            ctx.SavedChanges += Ctx_SavedChanges;

            return ctx;
        }

        private async Task CheckForPendingTasksAsync()
        {
            if (lastTask != null)
            {
                lastStatus = await lastTask;
                lastTask.Dispose();
                lastTask = null;
                if (lastStatus == 0)
                {
                    Restore();
                }
            }
        }

        private void Ctx_SavedChanges(object? sender, SavedChangesEventArgs e) =>
            lastTask = SynchronizeAsync();

        private async Task<int> SynchronizeAsync()
        {
            if (init)
            {
                Backup();
            }

            var result = await Js.InvokeAsync<int>(
                "db.synchronizeDbWithCache", backupName);
            var resultText = result == -1 ? "Failure" : (result == 0 ? "Restored" : "Cached");
            Console.WriteLine($"Synchronization status: {resultText}");
            return result;
        }

        private void Backup() => DoSwap(false);
        private void Restore() => DoSwap(true);

        private void DoSwap(bool restore)
        {
            backupName = restore ? backup : $"{backup}-{Guid.NewGuid().ToString().Split('-')[0]}";
            var dir = restore ? nameof(restore) : nameof(backup);
            Console.WriteLine($"Begin {dir}.");

            var source = restore ? $"Data Source={backupName}" : $"Data Source={dbFilename}";
            var target = restore ? $"Data Source={dbFilename}" : $"Data Source={backupName}";
            /*using var src = new SqliteConnection(source);
            using var tgt = new SqliteConnection(target);

            src.Open();
            tgt.Open();

            src.BackupDatabase(tgt);

            tgt.Close();
            src.Close();*/

            Console.WriteLine($"End {dir}.");
        }
    }
}
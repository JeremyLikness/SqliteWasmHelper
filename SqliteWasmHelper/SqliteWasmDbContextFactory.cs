using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SqliteWasmHelper
{
    public class SqliteWasmDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> dbContextFactory;
        private static readonly IDictionary<Type, string> fileNames = new Dictionary<Type, string>();
        private readonly BrowserCache cache;
        private Task<int>? startupTask = null;
        private int lastStatus = -2;
        private bool init = false;

        public SqliteWasmDbContextFactory(
            IDbContextFactory<TContext> dbContextFactory,
            BrowserCache cache)
        {
            this.cache = cache;
            this.dbContextFactory = dbContextFactory;
            startupTask = RestoreAsync();
        }

        public static string? GetFilenameForType() => 
            fileNames.ContainsKey(typeof(TContext)) ? fileNames[typeof(TContext)] : null;

        public async Task<TContext> CreateDbContextAsync()
        {
            await CheckForStartupTaskAsync();

            var ctx = await dbContextFactory.CreateDbContextAsync();

            if (!init)
            {
                await ctx.Database.EnsureCreatedAsync();
                init = true;
            }

            ctx.SavedChanges += (o, e) => Ctx_SavedChanges(ctx, e);

            return ctx;
        }

        private string Filename => fileNames[typeof(TContext)];
        private string BackupFile => $"{Filename}_bak";

        private string GetFilename()
        {
            using var ctx = dbContextFactory.CreateDbContext();
            var filename = "filenotfound.db";
            var type = ctx.GetType();
            if (fileNames.ContainsKey(type))
            {
                return fileNames[type];
            }
            var cs = ctx.Database.GetConnectionString();
            if (cs != null)
            {
                var file = cs.Split(';').Select(s => s.Split('='))
                    .Select(split => new
                    {
                        key = split[0].ToLowerInvariant(),
                        value = split[1]
                    })
                    .Where(kv => kv.key.Contains("data source") ||
                    kv.key.Contains("datasource") ||
                    kv.key.Contains("filename"))
                    .Select(kv => kv.value)
                    .FirstOrDefault();
                if (file != null)
                {
                    filename = file;
                }
            }
            fileNames.Add(type, filename);
            return filename;
        }

        private async Task CheckForStartupTaskAsync()
        {
            if (startupTask != null)
            {
                lastStatus = await startupTask;
                startupTask.Dispose();
                startupTask = null;
                
                if (lastStatus == 0)
                {
                    DoSwap(BackupFile, Filename);
                }
            }
        }

        private async void Ctx_SavedChanges(TContext ctx, SavedChangesEventArgs e)
        {
            await CheckForStartupTaskAsync();
            await ctx.Database.CloseConnectionAsync();
            if (e.EntitiesSavedCount > 0)
            {
                var backupName = $"{BackupFile}-{Guid.NewGuid().ToString().Split('-')[0]}";
                DoSwap(Filename, backupName);
                lastStatus = await cache.SyncDbWithCacheAsync(backupName);
            }
        }            

        private Task<int> RestoreAsync()
        {
            var filename = $"{GetFilename()}_bak";
            return cache.SyncDbWithCacheAsync(filename);
        }

        private static void DoSwap(string source, string target)
        {
            using var src = new SqliteConnection($"Data Source={source}");
            using var tgt = new SqliteConnection($"Data Source={target}");

            Console.WriteLine($"Backing up {source} to {target}");

            src.Open();
            tgt.Open();

            src.BackupDatabase(tgt);

            tgt.Close();
            src.Close();
        }
    }
}

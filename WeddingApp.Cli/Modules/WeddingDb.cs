using ConsoleAppFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using WeddingApp.Lib.Data;

namespace WeddingApp.Cli.Modules
{
    [Command("weddingdb", "Commands for working with the wedding DB.")]
    public class WeddingDb : ConsoleAppBase
    {
        private readonly WeddingDbContext _weddingDb;

        public WeddingDb(WeddingDbContext weddingDb) => _weddingDb = weddingDb;

        [Command("update", "Update the wedding DB to the latest migration.")]
        public async Task Update()
        {
            await _weddingDb.Database.MigrateAsync();
            Console.WriteLine("Wedding DB update complete.");
        }

        [Command("list-rsvps", "List all of the rsvps in the database.")]
        public async Task ListRsvps()
        {
            var rsvps = await _weddingDb.Rsvps.ToListAsync();
            foreach (var rsvp in rsvps)
            {
                Console.WriteLine(rsvp);
            }
            Console.WriteLine($"Total number of RSVPs: {rsvps.Count}");
        }

        [Command("export-rsvps", "Export the RSVPS from the database to the given file.")]
        public async Task ExportRsvps(
            [Option(0, "Path where the exported CSV file will be saved.")] string? filePath = null
        )
        {
            var rsvps = await _weddingDb.Rsvps.ToListAsync();
            using var stream = filePath is null
                ? System.Console.OpenStandardOutput()
                : File.OpenWrite(filePath);
            using var writer = new StreamWriter(stream);
            await writer.WriteAsync(Rsvp.ToCsv(rsvps));
        }

        [Command("rsvp-passphrase", "Get or set the RSVP passphrase.")]
        public async Task GetPassphrase(
            [Option(0)] string? newPassphrase = null
        )
        {
            var config = await _weddingDb.WebConfig();
            if (newPassphrase is null)
            {
                Console.WriteLine($"Stored passphrase: '{config.RsvpPassword}'");
            }
            else
            {
                config.RsvpPassword = newPassphrase;
                _weddingDb.Set<WebConfiguration>().Update(config);
                await _weddingDb.SaveChangesAsync();
            }
        }
    }
}

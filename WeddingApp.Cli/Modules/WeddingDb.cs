using ConsoleAppFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using WeddingApp.Lib.Data;

namespace WeddingApp.Cli.Modules
{
    [Command("weddigdb", "Commands for working with the wedding DB.")]
    public class WeddingDb : ConsoleAppBase
    {
        private readonly WeddingDbContext _weddingDb;

        public WeddingDb(WeddingDbContext weddingDb) => _weddingDb = weddingDb;

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
            [Option(0, "Path to the file were the exported CSV file will be saved.")] string filePath
        )
        {
            var rsvps = await _weddingDb.Rsvps.ToListAsync();
            using var writer = new StreamWriter(File.OpenWrite(filePath));
            await writer.WriteAsync(Rsvp.ToCsv(rsvps));
        }
    }
}

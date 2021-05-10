using ConsoleAppFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeddingApp.Lib.Data;
using WeddingApp.Lib.Services;

namespace WeddingApp.Cli.Modules
{
    [Command("weddingdb", "Commands for working with the wedding DB.")]
    public class WeddingDb : ConsoleAppBase
    {
        private readonly WeddingDbContext _weddingDb;
        private readonly EmailService _emailService;

        public WeddingDb(
            WeddingDbContext weddingDb,
            EmailService emailService)
        => (_weddingDb, _emailService) = (weddingDb, emailService);

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

        [Command("send-meeting-link", "Sends the meeting link to everyone in the database.")]
        public async Task SendMeetingLink()
        {
            var rsvps = await _weddingDb.Rsvps
                .Where(e => e.Name != null && e.Email != null)
                .ToListAsync();
            await Task.WhenAll(rsvps
                .Select(e => _emailService.SendMeetingLink(e.Name!, e.Email!)));
            
            Console.WriteLine("Sent emails to:");
            foreach (var rsvp in rsvps)
            {
                Console.WriteLine(rsvp.NameAndEmail());
            }
            Console.WriteLine($"Sent a total of {rsvps.Count} emails out of {await _weddingDb.Rsvps.CountAsync()} RSVPs.");

            var emailsSent = rsvps.Select(e => e.Email).ToArray();
            var rsvpsSkipped = _weddingDb.Rsvps.Where(e => !emailsSent.Contains(e.Email));
            Console.WriteLine("RSVPs that were skipped:");
            foreach (var rsvp in rsvpsSkipped)
            {
                Console.WriteLine(rsvp.NameAndEmail());
            }
        }
    }
}

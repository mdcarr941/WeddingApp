using ConsoleAppFramework;
using System;
using WeddingApp.Lib.Data;

namespace WeddingApp.Cli.Modules
{
    /// <summary>
    /// Commands for working with the wedding DB.
    /// </summary>
    public class WeddingDb
    {
        private readonly WeddingDbContext _weddingDb;

        public WeddingDb(WeddingDbContext weddingDb) => _weddingDb = weddingDb;

        [Command("list-rsvps", "List all of the rsvps in the database.")]
        public void ListRsvps()
        {
            foreach (var rsvp in _weddingDb.Rsvps)
            {
                Console.WriteLine(rsvp);
            }
        }
    }
}

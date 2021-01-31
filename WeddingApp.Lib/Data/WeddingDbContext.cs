using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeddingApp.Lib.Data
{
    public record Rsvp(string Email, string Name, bool EmailConfirmed = false)
    {
        public string ToCsv()
            => $"{Email},{Name},{EmailConfirmed}";

        public static string ToCsv(IEnumerable<Rsvp> rsvps)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{nameof(Email)},{nameof(Name)},{nameof(EmailConfirmed)}");
            foreach (var rsvp in rsvps)
            {
                builder.AppendLine(rsvp.ToCsv());
            }
            return builder.ToString();
        }
    }

    public class WeddingDbContext : DbContext
    {
        public static string ConnStringName { get; } = "WeddingDb";

        public DbSet<Rsvp> Rsvps { get; }

        public WeddingDbContext(DbContextOptions<WeddingDbContext> options)
            : base(options)
        {
            Rsvps = Set<Rsvp>();
        }

        /// <summary>
        /// Get the <see cref="WebConfiguration"/> stored in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<WebConfiguration> WebConfig()
        {
            var webConfig = await Set<WebConfiguration>().FirstOrDefaultAsync();
            if (webConfig is null)
            {
                webConfig = WebConfiguration.Default;
                Add(webConfig);
                await SaveChangesAsync();
            }
            return webConfig;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureRsvp(modelBuilder.Entity<Rsvp>());
            ConfigureWebConfiguration(modelBuilder.Entity<WebConfiguration>());
        }

        private void ConfigureRsvp(EntityTypeBuilder<Rsvp> entityBuilder)
        {
            entityBuilder.HasKey(e => e.Email);
        }

        private void ConfigureWebConfiguration(EntityTypeBuilder<WebConfiguration> entityBuilder)
        {
            entityBuilder.HasKey(e => e.Id);
        }
    }
}

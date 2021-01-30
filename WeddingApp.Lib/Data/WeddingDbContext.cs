using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeddingApp.Lib.Data
{
    public record Rsvp(int Pin, string Name, string Email, bool EmailConfirmed = false);

    public class WeddingDbContext : DbContext
    {
        public static string ConnStringName { get; } = "WeddingDb";

        public DbSet<Rsvp> Rsvps { get; }

        public WeddingDbContext(DbContextOptions<WeddingDbContext> options)
            : base(options)
        {
            Rsvps = Set<Rsvp>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureRsvp(modelBuilder.Entity<Rsvp>());
        }

        private void ConfigureRsvp(EntityTypeBuilder<Rsvp> entityBuilder)
        {
            entityBuilder.HasKey(e => e.Pin);
        }
    }
}

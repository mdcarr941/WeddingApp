using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;
using System.Threading.Tasks;

namespace WeddingApp.Lib.Data
{
    public class WeddingDbContext : DbContext
    {
        public static string ConnStringName { get; } = "WeddingDb";

        public DbSet<Rsvp> Rsvps { get; }

        public DbSet<EmailConfirmationCode> EmailConfirmationCodes { get; }

        public WeddingDbContext(DbContextOptions<WeddingDbContext> options)
            : base(options)
        {
            Rsvps = Set<Rsvp>();
            EmailConfirmationCodes = Set<EmailConfirmationCode>();
        }

        /// <summary>
        /// Get the <see cref="WebConfiguration"/> stored in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<WebConfiguration> WebConfig()
        {
            var webConfig = await Set<WebConfiguration>().FindAsync(WebConfiguration.SingletonId);
            if (webConfig is null)
            {
                webConfig = WebConfiguration.Default;
                Add(webConfig);
                await SaveChangesAsync();
            }
            return webConfig;
        }

        /// <summary>
        /// Gets or creates an <see cref="EmailConfirmationCode"/> for the given email address.
        /// </summary>
        public async Task<EmailConfirmationCode> EmailConfirmationCode(string email)
        {
            var confirmation = await EmailConfirmationCodes
                .Where(e => e.Email == email)
                .FirstOrDefaultAsync();
            if (confirmation is null)
            {
                confirmation = new EmailConfirmationCode(email);
                await EmailConfirmationCodes.AddAsync(confirmation);
                await SaveChangesAsync();
            }
            return confirmation;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureRsvp(modelBuilder.Entity<Rsvp>());
            ConfigureWebConfiguration(modelBuilder.Entity<WebConfiguration>());
            ConfigureEmailConfirmationCode(modelBuilder.Entity<EmailConfirmationCode>());
        }

        private void ConfigureRsvp(EntityTypeBuilder<Rsvp> entityBuilder)
        {
            entityBuilder.HasKey(e => e.Email);
            entityBuilder.Property(e => e.Name);
            entityBuilder.Property(e => e.EmailConfirmed);
            entityBuilder.Property(e => e.CreatedOnUtc);
        }

        private void ConfigureWebConfiguration(EntityTypeBuilder<WebConfiguration> entityBuilder)
        {
            entityBuilder.HasKey(e => e.Id);
        }

        private void ConfigureEmailConfirmationCode(EntityTypeBuilder<EmailConfirmationCode> entityBuilder)
        {
            entityBuilder.HasKey(e => e.Code);
            entityBuilder.HasOne(e => e.Rsvp)
                .WithOne(e => e.ConfirmationCode)
                .HasForeignKey<EmailConfirmationCode>(e => e.Email)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

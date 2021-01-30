using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeddingApp.Lib.Data;

namespace WeddingApp.Lib.Extensions
{
    /// <summary>
    /// Extensions to the DbContextOptionsBuilder class.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder ConfigureWeddingDbContext(
            this DbContextOptionsBuilder builder,
            IConfiguration config
        )
            => builder.UseSqlite(config.GetConnectionString(WeddingDbContext.ConnStringName));
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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
            => builder.UseSqlite(
                Environment.ExpandEnvironmentVariables(
                    config.GetConnectionString(WeddingDbContext.ConnStringName)));
    }
}

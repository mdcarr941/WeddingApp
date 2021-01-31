using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeddingApp.Lib.Data;

namespace WeddingApp.Lib.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddWeddingDbContext(
            this IServiceCollection services,
            IConfiguration configuration
        )
            => services.AddDbContext<WeddingDbContext>(options
                => options.ConfigureWeddingDbContext(configuration));
    }
}

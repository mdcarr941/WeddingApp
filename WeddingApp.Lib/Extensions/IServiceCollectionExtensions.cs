using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeddingApp.Lib.Data;
using WeddingApp.Lib.Services;
using System;

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

        public static IServiceCollection AddEmailService(this IServiceCollection services)
            => services.AddSingleton<EmailConfiguration>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>()
                    ?? throw new ArgumentNullException($"The {nameof(IConfiguration)} service is not registered.");
                return configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
            })
            .AddSingleton<EmailService>();
    }
}

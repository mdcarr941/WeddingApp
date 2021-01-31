using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using WeddingApp.Lib.Data;
using WeddingApp.Lib.Extensions;

namespace WeddingApp.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAppFrameworkAsync(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services)
                    => services.AddDbContext<WeddingDbContext>(options
                        => options.ConfigureWeddingDbContext(context.Configuration)));
    }
}

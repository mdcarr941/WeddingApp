using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
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
                    => services.AddWeddingDbContext(context.Configuration));
    }
}

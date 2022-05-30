
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        config.AddJsonFile("weaponarchetypes.json", true);
                        config.AddJsonFile("weaponcustomization.json", true);
                        config.AddJsonFile("guilds.json", true);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
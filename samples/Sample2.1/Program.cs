using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Json;

namespace SodaPop.ConfigExplorer.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(c =>
                {
                    // Remove non-JSON configuration sources
                    foreach (var source in c.Sources.Where(s => !(s is JsonConfigurationSource)).ToArray())
                    {
                        c.Sources.Remove(source);
                    }
                })
                .UseStartup<Startup>();
    }
}

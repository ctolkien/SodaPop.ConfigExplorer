using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SodaPop.ConfigExplorer.Sample
{
    public class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .SuppressStatusMessages(false)
                .UseStartup<Startup>();
    }
}

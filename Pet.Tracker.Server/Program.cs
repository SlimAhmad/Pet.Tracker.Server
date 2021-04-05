using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Dna;
using Dna.AspNet;

namespace Pet.Tracker.Server
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
                    // Add Dna Framework
                    webBuilder.UseDnaFramework(construct =>
                 {
                     // Configure framework

                     // Add file logger
                     construct.AddFileLogger();
                 })
                    .UseStartup<Startup>();
                });
      
    }
}

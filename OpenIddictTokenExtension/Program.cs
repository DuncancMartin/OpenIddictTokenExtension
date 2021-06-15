using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace OpenIddictTokenExtension
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(o => o.AllowSynchronousIO = true);
                    webBuilder.SuppressStatusMessages(true);
                    webBuilder.CaptureStartupErrors(false);
                    webBuilder.UseStartup<Startup>();
                });
    }
}

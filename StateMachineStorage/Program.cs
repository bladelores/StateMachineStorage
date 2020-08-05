using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace StateMachineStorage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                    services.Configure<KestrelServerOptions>(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Any, context.Configuration.GetValue<int>("Kestrel:HttpPort"));
                        serverOptions.Listen(IPAddress.Any, context.Configuration.GetValue<int>("Kestrel:HttpsPort"),
                        listenOptions =>
                        {
                            listenOptions.UseHttps();   
                        });
                    }))
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseStartup<Startup>();
        }
    }
}

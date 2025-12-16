using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using System.IO;


namespace NotificacaoPubSub.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("service", "notificacao-pubsub-api")
                .Enrich.WithProperty("env", "local")
                .WriteTo.File(
                    new CompactJsonFormatter(),
                    @"C:\logs\notificacao-api\log-.json",
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }


        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>()
                 .ConfigureLogging(logging =>
                 {
                     logging.ClearProviders(); // remove console, debug etc
                     logging.AddSerilog();     // conecta ILogger ao Serilog
                 })
                 .UseContentRoot(Directory.GetCurrentDirectory());
    }
}

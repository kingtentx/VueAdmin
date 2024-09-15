using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Globalization;
using System.IO;

namespace VueAdmin.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("zh-CN", true) { DateTimeFormat = { ShortDatePattern = "yyyy-MM-dd", FullDateTimePattern = "yyyy-MM-dd HH:mm:ss", LongTimePattern = "HH:mm:ss" } };

            Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
#else
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)          
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error) //����EF sql���
#endif
          //.MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
          .Enrich.FromLogContext()
          .Filter.ByExcluding(e =>
                e.Properties.TryGetValue("RequestPath", out var value) && (value.ToString() == "\"/layuiadmin\""))
          .WriteTo.Async(c => c.File($"Logs/.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 90))
          .WriteTo.Console()
          .CreateLogger();

            try
            {
                Log.Information("Starting VueAdmin.Web");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "MpOrder.Web terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog();


    }
}

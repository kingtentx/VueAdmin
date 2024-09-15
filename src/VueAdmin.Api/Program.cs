using Serilog;
using Serilog.Events;
using System.Globalization;
using System.Text;

namespace VueAdmin.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //注册字符集，一些中文编码数据为乱码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //使用中文时间格式
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("zh-CN", true) { DateTimeFormat = { ShortDatePattern = "yyyy-MM-dd", FullDateTimePattern = "yyyy-MM-dd HH:mm:ss", LongTimePattern = "HH:mm:ss" } };

            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#endif

#if !DEBUG
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error) //过滤EF sql输出               
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithProperty("Application", "Api")
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File($"Logs/.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 180))             
                .WriteTo.Console()
                .CreateLogger();

            CreateHostBuilder(args).UseConsoleLifetime().Build().Run();
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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using Data;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace TransportManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    CreateDbIfNotExists(services);
                }

                Log.Information("Starting web host");
                host.Run();
            }
            catch (Exception e)
            {
                Log.Error($"Host terminated unexpectedly - {e.Message}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfig) => 
                    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateDbIfNotExists(IServiceProvider services)
        {
            try
            {
                //регистрируем кодировку, чтобы получать корректные исключения от БД
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1251");

                Log.Information("Starting initialize DB");
                var context = services.GetRequiredService<DataContext>();
                DbInitializer.Initialize(context);
                Log.Information("DB was successfully initialized");
            }
            catch (Exception e)
            {
                Log.Error($"An error occurred creating the DB - {e.Message}");
            }
        }

        //ЗАДАНИЕ:
        //переписать предыдущий проект(только запросы, без статистик, для БД использовать EF и PostgreSQL) на ASP.
        //для маппинга использовать Automapper, добавить авторизацию/аутентификацию(желательно с JWT. описать принцип работы и настройки в README).

    }
}

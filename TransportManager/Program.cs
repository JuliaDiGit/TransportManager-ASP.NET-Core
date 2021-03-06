using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using TransportManager.Data;
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
                    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration)) // используем настройки через appsettings.json
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateDbIfNotExists(IServiceProvider services)
        {
            try
            {
                Log.Information("Starting initialize DB");
                var context = services.GetRequiredService<DataContext>();
                DbInitializer.Initialize(context);
                Log.Information("DB was successfully initialized");
            }
            catch (Exception e)
            {
                // при возникновении ошибки до подключения к БД используется некорректная кодировка,
                // поэтому данная ошибка переписана вручную
                Log.Error(e.Message.Contains("28P01:")
                          ? "An error occurred creating the DB - 28P01: invalid_password"
                          : $"An error occurred creating the DB - {e.Message}");
            }
        }
    }
}

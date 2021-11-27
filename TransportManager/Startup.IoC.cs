using TransportManager.Authorization;
using TransportManager.Authorization.Abstract;
using TransportManager.Authorization.AuthorizationServiceDecorators;
using TransportManager.Data.Repositories;
using TransportManager.Data.Repositories.Abstract;
using Microsoft.Extensions.DependencyInjection;
using TransportManager.Helpers;
using TransportManager.Services;
using TransportManager.Services.Abstract;
using TransportManager.Services.Decorators.CompaniesServiceDecorators;
using TransportManager.Services.Decorators.DriversServiceDecorators;
using TransportManager.Services.Decorators.UsersServiceDecorators;
using TransportManager.Services.Decorators.VehiclesServiceDecorators;

namespace TransportManager
{
    public partial class Startup
    {
        public void ConfigureIoC(IServiceCollection services)
        {
            //добавляем сервис и репо как Scoped, чтобы их объекты существовали на время запроса

            services.AddScoped<ICompaniesService, CompaniesService>();
            services.Decorate<ICompaniesService, CompaniesServiceLoggerDecorator>();
            services.AddScoped<ICompaniesRepository, CompaniesRepository>();

            services.AddScoped<IDriversService, DriversService>();
            services.Decorate<IDriversService, DriversServiceLoggerDecorator>();
            services.AddScoped<IDriversRepository, DriversRepository>();

            services.AddScoped<IVehiclesService, VehiclesService>();
            services.Decorate<IVehiclesService, VehiclesServiceLoggerDecorator>();
            services.AddScoped<IVehiclesRepository, VehiclesRepository>();

            services.AddScoped<IUsersService, UsersService>();
            services.Decorate<IUsersService, UsersServiceLoggerDecorator>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.Decorate<IAuthorizationService, AuthorizationServiceLoggerDecorator>();

        }
    }
}

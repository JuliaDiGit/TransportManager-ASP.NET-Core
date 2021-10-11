using Authorization;
using Authorization.Abstract;
using Authorization.AuthorizationServiceDecorators;
using Data.Repositories;
using Data.Repositories.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Abstract;
using Services.Decorators.CompaniesServiceDecorators;
using Services.Decorators.DriversServiceDecorators;
using Services.Decorators.UsersServiceDecorators;
using Services.Decorators.VehiclesServiceDecorators;
using TransportManager.Helpers;

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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using TransportManager.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace TransportManager
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DbConnection"), //берём строку подключения из appsettings.json
                                  assembly => {assembly.MigrationsAssembly("TransportManager");}); //настраиваем хранение миграций в текущей сборке
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]); //берём ключ из appsettings.json

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true, //валидация ключа безопасности
                            IssuerSigningKey = new SymmetricSecurityKey(key), //ключ безопасности
                            ValidateIssuer = false, //валидация издателя
                            ValidateAudience = false, //валидация потребителя
                            ValidateLifetime = true, // валидация времени существования
                            ClockSkew = TimeSpan.Zero // убираем смещение часов, чтобы токен жил ровно заданное время
                        };
                    });

            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));

            ConfigureIoC(services);

            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var path = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(path);

                // AddSecurityDefinition позволяет выбрать схему безопасности
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http, //тип схемы безопасности
                    Scheme = "bearer", //имя схемы, которое будет отображать в заголовке
                    In = ParameterLocation.Header, //расположение АПИ ключа
                    Description = "Поместите в данное поле токен, полученный при авторизации" //описание (необязательный параметр)
                });

                // AddSecurityRequirement позволяет применять схемы глобально (ко всем операциям с атрибутом Authorize) 
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" // название схемы, которая будет применяться 
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TransportManager API");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}

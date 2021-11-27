using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TransportManager.Common.Exceptions;
using TransportManager.Models;

namespace TransportManager
{
    /// <summary>
    ///     класс ExceptionHandlerMiddleware используется для обработки необработанных исключений
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            var result = JsonConvert.SerializeObject(GetExceptionViewModel(e));

            context.Response.ContentType = "application/json";

            switch (e)
            {
                case UserErrorException ex:
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                default:
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            return context.Response.WriteAsync(result);
        }

        private ExceptionViewModel GetExceptionViewModel(Exception e)
        {
            return new ExceptionViewModel()
            {
                ClassName = e.GetType().Name.Split('.').Reverse().First(),
                // если есть внутреннее исключение - разварачиваем его через рекурсию
                InnerException = e.InnerException != null ? GetExceptionViewModel(e.InnerException) : null,
                Message = e is Npgsql.PostgresException ? "Ошибка работы базы данных" : e.Message
            };
        }
    }
}
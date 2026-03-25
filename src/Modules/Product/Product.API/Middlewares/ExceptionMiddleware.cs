using BuildingBlocks.Domain.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Product.API.Middlewares
{
    // Tüm exception'ları yakalayan middleware
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Pipeline içinde çalışır
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        // Exception'ı uygun HTTP response'a çevirir
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;

            if (exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exception is BusinessRuleException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }

            var response = new
            {
                statusCode = (int)statusCode,
                message = exception.Message
            };

            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
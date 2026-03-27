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
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
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

            LogException(context, exception, statusCode);

            var response = new
            {
                statusCode = (int)statusCode,
                message = exception.Message
            };

            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }

        // Exception detaylarını loglar
        private void LogException(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(
                    exception,
                    "Beklenmeyen hata oluştu. Method: {Method}, Path: {Path}, StatusCode: {StatusCode}",
                    method,
                    path,
                    (int)statusCode);
                return;
            }

            _logger.LogWarning(
                exception,
                "İşlemsel hata oluştu. Method: {Method}, Path: {Path}, StatusCode: {StatusCode}, Message: {Message}",
                method,
                path,
                (int)statusCode,
                exception.Message);
        }

    }
}
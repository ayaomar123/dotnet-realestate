using System.Net;
using System.Text.Json;
using Realestate.DTOs;

namespace Realestate.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<string>.Fail("An unexpected error occurred.");
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }

            // توحيد 400 / 401 / 403 حتى لو ما صار Exception
            if (!context.Response.HasStarted)
            {
                switch (context.Response.StatusCode)
                {
                    case 400:
                        await WriteStandardError(context, "Bad Request.");
                        break;
                    case 401:
                        await WriteStandardError(context, "Unauthorized: Invalid or missing token.");
                        break;
                    case 403:
                        await WriteStandardError(context, "Forbidden: You do not have access.");
                        break;
                }
            }
        }

        private async Task WriteStandardError(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";

            var response = ApiResponse<string>.Fail(message);
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}

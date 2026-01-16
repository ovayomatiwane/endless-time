using Common.Responses;
using Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace WebApi
{
    public class ExceptionHandlingMiddleware
    {
        public readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EntityNotFoundException ex)
            {
                await WriteError(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                await WriteError(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (RequiredNullOrEmptyStringException ex)
            {
                await WriteError(context, HttpStatusCode.InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                await WriteError(context, HttpStatusCode.InternalServerError,
                    "An unexpected error occurred",
                    ex.Message);
            }
        }

        private static async Task WriteError(
            HttpContext context,
            HttpStatusCode statusCode,
            string message,
            params string[] errors)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = ApiResponse.Fail(message, errors);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

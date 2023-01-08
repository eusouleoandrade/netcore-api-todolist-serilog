using Core.Application.Dtos.Wrappers;
using Core.Application.Exceptions;
using Core.Application.Resources;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace Presentation.WebApi.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception error)
            {
                string message;

                switch (error)
                {
                    case AppException:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        message = error.Message;

                        break;
                    default:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        message = Msg.INTERNAL_SERVER_ERROR_TXT;

                        break;
                }

                var response = new Response(succeeded: false, message);

                var serializedResponse = JsonSerializer.Serialize(response);

                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(serializedResponse);
            }
        }
    }
}
using Presentation.WebApi.Middlewares;

namespace Presentation.WebApi.Extensions
{
    public static class HttpRequestLoggerHandlerExtension
    {
        public static void UseHttpRequestHandlingExtension(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            app.UseMiddleware<HttpRequestLoggerHandlerMiddleware>();
        }
    }
}
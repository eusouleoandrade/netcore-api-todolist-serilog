using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Presentation.WebApi.Options;

namespace Presentation.WebApi.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class CorrelationIdHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdOptions _options;

        public CorrelationIdHandlerMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(_options.Header, out StringValues correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            // Add correlationId in the traceIdentifier of httpContext
            httpContext.TraceIdentifier = correlationId;

            // Aplica o correlationId ao cabeçalho de resposta para rastreamento lado cliente
            if (_options.IncludeInResponse)
            {
                httpContext.Response.OnStarting(() =>
                {
                    httpContext.Response.Headers.Add(_options.Header, new[] { correlationId.ToString() });
                    return Task.CompletedTask;
                });
            }

            await _next(httpContext);
        }
    }
}
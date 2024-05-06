using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Common.Logging.Correlation
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private const string _correlationIdHeader = "X-Correlation-Id";
        
        public CorrelationIdMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext, ICorrelationIdGenerator correlationIdGenerator) 
        {
            var correlationId = GetCorrelationId(httpContext, correlationIdGenerator);
            AddCorrelationIdHeader(httpContext, correlationId);
            await _requestDelegate(httpContext);
        }

        private StringValues GetCorrelationId(HttpContext httpContext, ICorrelationIdGenerator correlationIdGenerator)
        {
            if(httpContext.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId)) 
            {
                correlationIdGenerator.Set(correlationId);
                return correlationId;
            }

            return correlationIdGenerator.Get();
        }

        private void AddCorrelationIdHeader(HttpContext httpContext, StringValues correlationId)
        {
            httpContext.Response.OnStarting(() => 
            {
                httpContext.Response.Headers.Add(_correlationIdHeader, new[] { correlationId.ToString() });
                return Task.CompletedTask;
            });
        }
    }
}

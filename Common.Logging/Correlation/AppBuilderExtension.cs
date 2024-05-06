using Microsoft.AspNetCore.Builder;

namespace Common.Logging.Correlation
{
    public static class AppBuilderExtension 
    {
        public static IApplicationBuilder AddCorrelationMiddleware(this IApplicationBuilder applicationBuilder) 
            => applicationBuilder.UseMiddleware<CorrelationIdMiddleware>();
    }
}

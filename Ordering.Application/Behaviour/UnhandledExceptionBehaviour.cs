using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviour
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            this.logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception)
            {
                var requestName = typeof(TRequest).Name;
                logger.LogError($"Unhandled exception occured during request ({requestName}, {request})");
                throw;
            }
        }
    }
}

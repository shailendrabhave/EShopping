using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.logger = logger;
        }
        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await orderRepository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }
            await orderRepository.DeleteAsync(orderToDelete);
            logger.LogInformation($"Order ({request.Id}) successfully deleted");
        }
    }
}

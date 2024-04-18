using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CheckoutOrderCommandHandler> logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = mapper.Map<Order>(request);
            var generatedOrder = await orderRepository.AddAsync(orderEntity);
            logger.LogInformation($"Order generated: {generatedOrder}");
            return generatedOrder.Id;
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ordering.Application.Handlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateOrderCommandHandler> logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await orderRepository.GetByIdAsync(request.Id);
            if(orderToUpdate == null)
            {
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }
            mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));
            await orderRepository.UpdateAsync(orderToUpdate);
            logger.LogInformation($"Order ({orderToUpdate.Id}) successfully updated");
        }
    }
}

using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<BasketCheckoutEvent> logger;

        public BasketOrderingConsumer(IMediator mediator, IMapper mapper, ILogger<BasketCheckoutEvent> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var checkoutBasketCommand = mapper.Map<CheckoutOrderCommand>(context.Message);
            await mediator.Send(checkoutBasketCommand);
            logger.LogInformation($"Bakset checkout completed");

        }
    }
}

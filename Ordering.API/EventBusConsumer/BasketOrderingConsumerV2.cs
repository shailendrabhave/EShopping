using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<BasketOrderingConsumerV2> logger;

        public BasketOrderingConsumerV2(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumerV2> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            using var scope = logger.BeginScope(
                "Consuming Basket Checkout V2 event for Correlation Id: {CorrelationId}", 
                context.Message.CorrelationId);

            var checkoutBasketCommand = mapper.Map<CheckoutOrderCommand>(context.Message);
            PopulateAddressDetails(checkoutBasketCommand);

            await mediator.Send(checkoutBasketCommand);
            logger.LogInformation("Bakset checkout completed");
        }

        private static void PopulateAddressDetails(CheckoutOrderCommand command)
        {
            command.FirstName = "Rahul";
            command.LastName = "Sahay";
            command.EmailAddress = "rahulsahay@eshop.net";
            command.AddressLine = "Bangalore";
            command.Country = "India";
            command.State = "KA";
            command.ZipCode = "560001";
            command.PaymentMethod = 1;
            command.CardName = "Visa";
            command.CardNumber = "1234567890123456";
            command.Expiration = "12/25";
            command.Cvv = "123";
        }
    }
}

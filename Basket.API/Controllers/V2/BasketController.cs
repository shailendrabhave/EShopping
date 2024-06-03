using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ICorrelationIdGenerator correlationIdGenerator;

        public BasketController(
            IMediator mediator,
            IPublishEndpoint publishEndpoint,
            ILogger<BasketController> logger,
            ICorrelationIdGenerator correlationIdGenerator)
        {
            this.mediator = mediator;
            this.publishEndpoint = publishEndpoint;
            this.correlationIdGenerator = correlationIdGenerator;
            logger.LogInformation("Correlation Id:{CorrelationId}", correlationIdGenerator.Get());
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
        {
            var getBasketQuery = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await mediator.Send(getBasketQuery);
            if (basket == null)
                return BadRequest();

            var eventMessage = BasketMapper.MapperExt.Map<BasketCheckoutEventV2>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            eventMessage.CorrelationId = correlationIdGenerator.Get();
            await publishEndpoint.Publish(eventMessage);

            //remove the basket
            var deleteBasketCommand = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await mediator.Send(deleteBasketCommand);

            return Accepted();
        }
    }
}

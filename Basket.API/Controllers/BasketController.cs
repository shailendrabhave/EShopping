using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    public class BasketController:APIController
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

        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasketByUserName(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> CreateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
        {
            var basket = await mediator.Send(createShoppingCartCommand);
            return Ok(basket);
        }

        [HttpDelete]
        [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasketByUserName(string userName)
        {
            var deleteBasketByUserNameCommand = new DeleteBasketByUserNameCommand(userName);
            await mediator.Send(deleteBasketByUserNameCommand);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody]BasketCheckout basketCheckout)
        {
            var getBasketQuery = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await mediator.Send(getBasketQuery);
            if (basket == null)
                return BadRequest();

            var eventMessage = BasketMapper.MapperExt.Map<BasketCheckoutEvent>(basketCheckout);
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

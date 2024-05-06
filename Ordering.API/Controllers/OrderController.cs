using Common.Logging.Correlation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using System.Net;

namespace Ordering.API.Controllers
{
    public class OrderController : APIController
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator, ILogger<OrderController> logger, ICorrelationIdGenerator correlationIdGenerator)
        {
            this.mediator = mediator;
            logger.LogInformation("Correlation Id:{CorrelationId}", correlationIdGenerator.Get());
        }

        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(IEnumerable<OrderReponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderReponse>>>GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersQuery(userName);
            var orders = await mediator.Send(query);
            return Ok(orders);
        }

        [HttpPost("CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody]CheckoutOrderCommand checkoutOrderCommand)
        {
            var result = await mediator.Send(checkoutOrderCommand);
            return Ok(result);
        }

        [HttpPut("UpdateOrder")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand updateOrderCommand)
        {
            await mediator.Send(updateOrderCommand);
            return NoContent();
        }

        [HttpDelete("[action]/{id}", Name = "DeleteOrder")]

        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var deleteOrderCommand = new DeleteOrderCommand(id);
            await mediator.Send(deleteOrderCommand);
            return NoContent();
        }
    }
}

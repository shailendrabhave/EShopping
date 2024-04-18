using MediatR;

namespace Basket.Application.Commands
{
    public class DeleteBasketByUserNameCommand:IRequest
    {
        public DeleteBasketByUserNameCommand(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; }
    }
}

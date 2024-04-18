using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers
{
    internal class DeleteBasketByUserNameHandler : IRequestHandler<DeleteBasketByUserNameCommand>
    {
        private readonly IBasketRepository basketRepository;

        public DeleteBasketByUserNameHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        public async Task Handle(DeleteBasketByUserNameCommand request, CancellationToken cancellationToken)
        {
            await basketRepository.DeleteBasket(request.UserName);
        }
    }
}

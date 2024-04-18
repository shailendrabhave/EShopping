using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers
{
    public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository basketRepository;
        private readonly DiscountService discountService;
        public CreateShoppingCartHandler(IBasketRepository basketRepository, DiscountService discountService) {
            this.basketRepository = basketRepository;
            this.discountService = discountService;
        }
        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Items)
            {
                var coupon = await discountService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            var shoppingCart = await basketRepository.UpdateBasket(new ShoppingCart(
                request.UserName,
                request.Items
            ));
            var shoppingCartResponse = BasketMapper.MapperExt.Map<ShoppingCartResponse>(shoppingCart);
            return shoppingCartResponse;
        }
    }
}

using Discount.Application.Commands;
using Discount.Application.Mappers;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Handlers
{
    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository discountRepository;

        public CreateDiscountCommandHandler(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }
        public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = DiscountMapper.MapperExt.Map<Coupon>(request);
            await discountRepository.CreateDiscount(coupon);
            var couponModel = DiscountMapper.MapperExt.Map<CouponModel>(coupon);
            return couponModel;
        }
    }
}

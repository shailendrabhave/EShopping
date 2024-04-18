using Discount.Application.Commands;
using Discount.Application.Mappers;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Handlers
{
    internal class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository discountRepository;

        public UpdateDiscountCommandHandler(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }
        public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = DiscountMapper.MapperExt.Map<Coupon>(request);
            await discountRepository.UpdateDiscount(coupon);
            var couponModel = DiscountMapper.MapperExt.Map<CouponModel>(coupon);
            return couponModel;
        }
    }
}

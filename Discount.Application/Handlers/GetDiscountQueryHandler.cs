using AutoMapper;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handlers
{
    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IDiscountRepository discountRepository;

        public GetDiscountQueryHandler(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }

        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var coupon = await discountRepository.GetDiscount(request.ProductName);
            if(coupon == null) 
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount not found for product: {request.ProductName}"));
            }
            var couponModel = DiscountMapper.MapperExt.Map<CouponModel>(coupon);
            return couponModel;
        }
    }
}

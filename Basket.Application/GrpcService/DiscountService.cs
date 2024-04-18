using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService
{
    public class DiscountService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient;

        public DiscountService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            this.discountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            return await discountProtoServiceClient.GetDiscountAsync(discountRequest);
        }
    }
}

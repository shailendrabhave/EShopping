using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Commands
{
    public class CreateDiscountCommand: IRequest<CouponModel>
    {
        public string ProductName { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int Amount { get; set; }
    }
}

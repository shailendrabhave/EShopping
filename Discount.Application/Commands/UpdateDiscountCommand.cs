using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Commands
{
    public class UpdateDiscountCommand:IRequest<CouponModel>
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int Amount { get; set; }
    }
}

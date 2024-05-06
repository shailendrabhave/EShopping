using Common.Logging.Correlation;
using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services
{
    public class DiscountService: DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<DiscountService> logger;

        public DiscountService(IMediator mediator, ILogger<DiscountService> logger, ICorrelationIdGenerator correlationIdGenerator)
        {
            this.mediator = mediator;
            this.logger = logger;
            logger.LogInformation("Correlation Id:{CorrelationId}", correlationIdGenerator.Get());
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery(request.ProductName);
            var result = await mediator.Send(query);
            logger.LogInformation("Discount for product ({ProductName}) is {Amount}", request.ProductName, result.Amount);
            return result;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var command = new CreateDiscountCommand() { 
                ProductName = request.Coupon.ProductName, 
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description
            };

            var result = await mediator.Send(command);
            logger.LogInformation("Discount for product ({ProductName}) created successfully.", result.ProductName);
            return result;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var command = new UpdateDiscountCommand()
            {
                Id = request.Coupon.Id,
                ProductName = request.Coupon.ProductName,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description
            };

            var result = await mediator.Send(command);
            logger.LogInformation("Discount for product ({ProductName}) updated successfully.", result.ProductName);
            return result;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var command = new DeleteDiscountCommand()
            {
                ProductName = request.ProductName,
            };

            var deleteed = await mediator.Send(command);
            logger.LogInformation("Discount for product ({ProductName}) deleted successfully.", request.ProductName);
            var response = new DeleteDiscountResponse { Success = deleteed };
            return response;
        }
    }
}

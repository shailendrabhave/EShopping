using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderReponse>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public GetOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<List<OrderReponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetOrderByUserName(request.UserName);
            return mapper.Map<List<OrderReponse>>(orders);
        }
    }
}

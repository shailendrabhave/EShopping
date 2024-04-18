using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductsByNameHandler : IRequestHandler<GetProductsByNameQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository productRepository;

        public GetProductsByNameHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IList<ProductResponse>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
        {
            var productList = await productRepository.GetProductByName(request.Name);
            var productResponseList = CatalogMapper.MapperExt.Map<IList<ProductResponse>>(productList.ToList());
            return productResponseList;
        }
    }
}

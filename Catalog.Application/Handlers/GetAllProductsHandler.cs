using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Handlers
{
    internal class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository productRepository;

        public GetAllProductsHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productList = await productRepository.GetAllProducts(request.CatalogSpecsParams);
            var productResponseList = CatalogMapper.MapperExt.Map<Pagination<ProductResponse>>(productList);
            return productResponseList;
        }
    }
}
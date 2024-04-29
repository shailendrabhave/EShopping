using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductsByBrandHandler : IRequestHandler<GetProductsByBrandQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository productRepository;

        public GetProductsByBrandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IList<ProductResponse>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var productList = await productRepository.GetProductByBrand(request.BrandName);
            var productResponseList = CatalogMapper.MapperExt.Map<IList<ProductResponse>>(productList.ToList());
            return productResponseList;
        }
    }
}

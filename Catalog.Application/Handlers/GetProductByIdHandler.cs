using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IProductRepository productRepository;

        public GetProductByIdHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetProduct(request.Id);
            var productResponse = CatalogMapper.MapperExt.Map<ProductResponse>(product);
            return productResponse;
        }
    }
}

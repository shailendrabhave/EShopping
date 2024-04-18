using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IProductRepository productRepository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = CatalogMapper.MapperExt.Map<Product>(request);
            if (productEntity == null)
                throw new ApplicationException("Issue with mapping while creating new product");

            var newProduct = await productRepository.CreateProduct(productEntity);
            var productResponse = CatalogMapper.MapperExt.Map<ProductResponse>(newProduct);
            return productResponse;
        }
    }
}

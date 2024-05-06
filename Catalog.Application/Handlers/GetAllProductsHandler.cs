using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers
{
    internal class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<GetAllProductsHandler> logger;

        public GetAllProductsHandler(IProductRepository productRepository, ILogger<GetAllProductsHandler> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var productList = await productRepository.GetAllProducts(request.CatalogSpecsParams);
                var productResponseList = CatalogMapper.MapperExt.Map<Pagination<ProductResponse>>(productList);

                var log = $"{productResponseList.Count} products fetched";
                logger.LogDebug(log);

                return productResponseList;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error has occured {Exception}", ex.Message);
                throw;
            }
        }
    }
}
using AutoMapper;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<ProductBrandResponse>>
    {
        private readonly IBrandRepository brandRepository;

        public GetAllBrandsHandler(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        public async Task<IList<ProductBrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brandList = await brandRepository.GetAllBrands();
            var brandResponseList = CatalogMapper.MapperExt.Map<IList<ProductBrandResponse>>(brandList.ToList());
            return brandResponseList;
        }
    }
}

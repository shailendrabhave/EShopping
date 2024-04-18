using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllProductTypesHandler : IRequestHandler<GetAllProductTypesQuery, IList<ProductTypeResponse>>
    {
        private readonly ITypesRepository typesRepository;

        public GetAllProductTypesHandler(ITypesRepository typesRepository)
        {
            this.typesRepository = typesRepository;
        }

        public async Task<IList<ProductTypeResponse>> Handle(GetAllProductTypesQuery request, CancellationToken cancellationToken)
        {
            var productTypesList = await typesRepository.GetAllProductTypes();
            var productTypeResponseList = CatalogMapper.MapperExt.Map<IList<ProductTypeResponse>>(productTypesList.ToList());
            return productTypeResponseList;
        }
    }
}

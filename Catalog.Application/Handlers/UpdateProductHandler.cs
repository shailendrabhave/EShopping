using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class UpdateProductHandler:IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository productRepository;

        public UpdateProductHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var updateProductResponse = await productRepository.UpdateProduct(new Product {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Summary = request.Summary,
                ImageFile = request.ImageFile,
                Price = request.Price,
                Brand = request.Brand,
                Type = request.Type
            });

            return updateProductResponse;
        }
    }
}

using Catalog.Application.Commands;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class DeleteProductHandler:IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await productRepository.DeleteProduct(request.Id);
        }
    }
}

using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Pagination<Product>> GetAllProducts(CatalogSpecsParams catalogSpecsParams);

        Task<Product> GetProduct(string id);

        Task<IEnumerable<Product>> GetProductByName(string name);

        Task<IEnumerable<Product>> GetProductByBrand(string brand);

        Task<Product> CreateProduct(Product product);

        Task<bool> UpdateProduct(Product product);

        Task<bool> DeleteProduct(string id);
    }
}

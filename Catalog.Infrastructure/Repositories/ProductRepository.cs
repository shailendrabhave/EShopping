using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await catalogContext.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(product => product.Id, id);
            var deleteResult = await catalogContext.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await catalogContext.ProductBrands.Find(productBrand => true).ToListAsync();
        }

        public async Task<Pagination<Product>> GetAllProducts(CatalogSpecsParams catalogSpecsParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!String.IsNullOrEmpty(catalogSpecsParams.Search))
            {
                var searchFilter = builder.Regex(x => x.Name, new BsonRegularExpression(catalogSpecsParams.Search));
                filter &= searchFilter;
            }

            if (!String.IsNullOrEmpty(catalogSpecsParams.BrandId))
            {
                var brandFilter = builder.Eq(x => x.Brand.Id, catalogSpecsParams.BrandId);
                filter &= brandFilter;
            }

            if (!String.IsNullOrEmpty(catalogSpecsParams.TypeId))
            {
                var typeFilter = builder.Eq(x => x.Type.Id, catalogSpecsParams.TypeId);
                filter &= typeFilter;
            }

            if (!string.IsNullOrEmpty(catalogSpecsParams.Sort))
            {
                return new Pagination<Product>()
                {
                    PageSize = catalogSpecsParams.PageSize,
                    PageIndex = catalogSpecsParams.PageIndex,
                    Data = await FilterProductsData(catalogSpecsParams, filter),
                    Count = await catalogContext.Products.CountDocumentsAsync(p => true)        //Need to check while developing UI
                };
            }

            var data = new Pagination<Product>()
            {
                PageSize = catalogSpecsParams.PageSize,
                PageIndex = catalogSpecsParams.PageIndex,
                Data = await catalogContext
                        .Products
                        .Find(filter)
                        .Sort(Builders<Product>.Sort.Ascending("Name"))
                        .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
                        .Limit(catalogSpecsParams.PageSize)
                        .ToListAsync(),
                Count = await catalogContext.Products.CountDocumentsAsync(p => true)
            };

            return data;
        }

        private async Task<IReadOnlyList<Product>> FilterProductsData(CatalogSpecsParams catalogSpecsParams, FilterDefinition<Product> filter)
        {
            switch (catalogSpecsParams.Sort)
            {
                case  "priceAsc":
                    return await catalogContext
                         .Products
                         .Find(filter)
                         .Sort(Builders<Product>.Sort.Ascending("Price"))
                         .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
                         .Limit(catalogSpecsParams.PageSize)
                         .ToListAsync();
                case "priceDesc":
                    return await catalogContext
                         .Products
                         .Find(filter)
                         .Sort(Builders<Product>.Sort.Descending("Price"))
                         .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
                         .Limit(catalogSpecsParams.PageSize)
                         .ToListAsync();
                default:
                    return await catalogContext
                         .Products
                         .Find(filter)
                         .Sort(Builders<Product>.Sort.Ascending("Name"))
                         .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
                         .Limit(catalogSpecsParams.PageSize)
                         .ToListAsync();                    
            }
        }

        public async Task<IEnumerable<ProductType>> GetAllProductTypes()
        {
            return await catalogContext.ProductTypes.Find(productType => true).ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await catalogContext.Products.Find((Product product) => product.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrand(string brand)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(product => product.Brand.Name, brand);
            return await catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(product => product.Name, name);
            return await catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await catalogContext.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}

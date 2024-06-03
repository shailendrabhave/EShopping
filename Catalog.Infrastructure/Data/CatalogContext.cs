using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public IMongoCollection<ProductBrand> ProductBrands { get; }

        public IMongoCollection<ProductType> ProductTypes { get; }

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>(key: "DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>(key: "DatabaseSettings:DatabaseName"));
            
            ProductBrands = database.GetCollection<ProductBrand>(
                configuration.GetValue<string>(key: "DatabaseSettings:BrandsCollection"));
            ProductTypes = database.GetCollection<ProductType>(
                configuration.GetValue<string>(key: "DatabaseSettings:TypesCollection"));
            Products = database.GetCollection<Product>(
                configuration.GetValue<string>(key: "DatabaseSettings:ProductsCollection"));

            BrandContextSeed.SeedData(ProductBrands);
            TypeContextSeed.SeedData(ProductTypes);
            CatalogContextSeed.SeedData(Products);
        }
    }
}

using Catalog.Core.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.Application.Responses
{
    public class ProductResponse
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public ProductBrand Brand { get; set; }
        public ProductType Type { get; set; }
        public decimal Price { get; set; }
    }
}

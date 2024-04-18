using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    public class CatalogController : APIController
    {
        private readonly IMediator mediator;

        public CatalogController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route(template: "[action]/{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route(template: "[action]/{productName}", Name = "GetProductsByProductName")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductsByProductName(string productName)
        {
            var query = new GetProductsByNameQuery(productName);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route(template: "[action]/{brandName}", Name = "GetProductsByBrandName")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductsByBrandName(string brandName)
        {
            var query = new GetProductsByBrandQuery(brandName);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetAllProducts([FromQuery] CatalogSpecsParams catalogSpecsParams)
        {
            var query = new GetAllProductsQuery(catalogSpecsParams);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProductBrands")]
        [ProducesResponseType(typeof(IList<ProductBrandResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductBrandResponse>>> GetAllProductBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProductTypes")]
        [ProducesResponseType(typeof(IList<ProductTypeResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductTypeResponse>>> GetAllProductTypes()
        {
            var query = new GetAllProductTypesQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand productCommand)
        {
            var result = await mediator.Send(productCommand);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand productCommand)
        {
            var result = await mediator.Send(productCommand);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var deleteProductCommand = new DeleteProductCommand() { Id = id };
            var result = await mediator.Send(deleteProductCommand);
            return Ok(result);
        }
    }
}

using catalog.Application.Commands;
using catalog.Application.Queries;
using catalog.Application.Responses;
using catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace catalog.API.Controllers
{

    public class CatalogController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IMediator mediator, ILogger<CatalogController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]/{Id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductResponeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult<ProductResponeDto>> GetProductById(string Id)
        {
            var query = new GetAllProductsByIdQuery(Id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{Name}", Name = "GetProductByName")]
        [ProducesResponseType(typeof(ProductResponeDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult<ProductResponeDto>> GetProductByName(string Name)
        {
            var query = new GetProductByNameQuery(Name);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        [Route("[action]/{Name}", Name = "GetAllProductsByName")]
        [ProducesResponseType(typeof(List<ProductResponeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult<ProductResponeDto>> GetAllProductsByName(string Name)
        {
            var query = new GetAllProductsByNameQuery(Name);
            var result = await _mediator.Send(query);
            _logger.LogInformation("GetAllProductsByName query executed with name: {Name}", Name);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(List<ProductResponeDto>), (int)HttpStatusCode.OK)]


        public async Task<ActionResult<List<ProductResponeDto>>> GetAllProducts([FromQuery]CatalogSpecsParams specsParams)
        {
            var query = new GetAllProductQuery(specsParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(typeof(List<BrandResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<BrandResponseDto>>> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllTypes")]
        [ProducesResponseType(typeof(List<TypesResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<TypesResponseDto>>> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }   

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponeDto), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<ProductResponeDto>> CreateProduct([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<bool>> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

        }

        [HttpPut]
        [Route("{Id}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<bool>> DeleteProduct(string Id)
        {
            var command = new DeleteProductCommand(Id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

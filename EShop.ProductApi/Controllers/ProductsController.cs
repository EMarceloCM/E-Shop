using EShop.ProductApi.DTOs;
using EShop.ProductApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var Dto = await _service.GetProducts();
            if (Dto == null)
            {
                return NotFound("Products Not Found!");
            }
            return Ok(Dto);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var Dto = await _service.GetProductById(id);
            if (Dto == null)
            {
                return NotFound("Product Not Found!");
            }
            return Ok(Dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Post([FromBody] ProductDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid Data");
            }
            await _service.AddProduct(dto);
            return new CreatedAtRouteResult("GetProduct", new {id = dto.Id }, dto);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put([FromBody] ProductDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid Data");
            await _service.UpdateProduct(dto);
            return Ok(dto);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var dto = await _service.GetProductById(id);
            if (dto == null)
                return NotFound("Product Not Found!");

            await _service.RemoveProduct(id);
            return Ok(dto);
        }
    }
}

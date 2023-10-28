using EShop.CartApi.DTOs;
using EShop.CartApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.CartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;

        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("getcard/{id:int}")]
        public async Task<ActionResult<CartDTO>> GetByUserId(string id)
        {
            var cartDto = await _repository.GetCartByUserIdAsync(id);
            
            if (cartDto == null) return NotFound();
            return Ok(cartDto);
        }

        [HttpPost("addcart")]
        public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartDTO)
        {
            var cart = await _repository.UpdateCartAsync(cartDTO);

            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPut("updatecart")]
        public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO cartDTO)
        {
            var cart = await _repository.UpdateCartAsync(cartDTO);

            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpDelete("deletecart/{id:int}")]
        public async Task<ActionResult<bool>> DeleteCart(int id)
        {
            var status = await _repository.DeleteItemCartAsync(id);

            if (!status) return BadRequest();
            return Ok(status);
        }
    }
}
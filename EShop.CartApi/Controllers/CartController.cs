using EShop.CartApi.DTOs;
using EShop.CartApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.CartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;

        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderDTO>> Checkout(CheckoutHeaderDTO checkoutDTO)
        {
            var cart = await _repository.GetCartByUserIdAsync(checkoutDTO.UserId);
            if (cart == null)
            {
                return NotFound("Not Found");
            }
            checkoutDTO.CartItems = cart.CartItems;
            checkoutDTO.DateTime = DateTime.UtcNow;
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

        [HttpPost("applycoupon")]
        public async Task<ActionResult<CartDTO>> ApplyCoupon(CartDTO cartDTO)
        {
            var result = await _repository.ApplyCouponAsync(cartDTO.CartHeader.UserId, cartDTO.CartHeader.CouponCode);

            if (!result)
            {
                return NotFound($"CartHeader not found for UserId = {cartDTO.CartHeader.UserId}");
            }
            return Ok(result);
        }

        [HttpDelete("deletecoupon/{userId}")]
        public async Task<ActionResult<CartDTO>> DeleteCoupon(string userId)
        {
            var result = await _repository.DeleteCouponAsync(userId);

            if (!result)
            {
                return NotFound($"Discount Coupon not found for UserId = {userId}");
            }
            return Ok(result);
        }
    }
}
namespace EShop.CartApi.DTOs
{
    public class CartDTO
    {
        public CartHeaderDTO CartHeader { get; set; } = new();
        public IEnumerable<CartItemDTO> CartItems { get; set; } = Enumerable.Empty<CartItemDTO>();
    }
}

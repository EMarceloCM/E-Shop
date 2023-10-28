using System.ComponentModel.DataAnnotations;

namespace EShop.CartApi.DTOs
{
    public class CartHeaderDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The User is Required")]
        public string UserId { get; set; } = string.Empty;
        public string CouponCode { get; set; } = string.Empty;
    }
}

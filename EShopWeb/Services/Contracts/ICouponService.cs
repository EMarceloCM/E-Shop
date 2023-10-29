using EShopWeb.Models;

namespace EShopWeb.Services.Contracts
{
    public interface ICouponService
    {
        Task<CouponViewModel> GetDiscountCoupon(string couponCode, string token);
    }
}

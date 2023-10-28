using EShop.DiscountApi.DTOs;

namespace EShop.DiscountApi.Repositories
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCode(string couponCode);
    }
}

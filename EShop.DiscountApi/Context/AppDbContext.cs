using EShop.DiscountApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.DiscountApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                CouponCode = "ESHOP_PROMO_10",
                Discount = 10
            });
            mb.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CouponCode = "ESHOP_PROMO_20",
                Discount = 20
            });
        }
    }
}

using EShop.CartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.CartApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Product>? Products { get; set; } 
        public DbSet<CartItem> CartItems { get; set; } 
        public DbSet<CartHeader> CartHeaders { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Product>().HasKey(p => p.Id);
            mb.Entity<Product>().Property(p => p.Id).ValueGeneratedNever();
            mb.Entity<Product>().Property(p => p.Name).HasMaxLength(100).IsRequired();
            mb.Entity<Product>().Property(p => p.Description).HasMaxLength(255).IsRequired();
            mb.Entity<Product>().Property(p => p.CategoryName).HasMaxLength(100).IsRequired();
            mb.Entity<Product>().Property(p => p.Price).HasPrecision(12, 2).IsRequired();

            mb.Entity<CartHeader>().Property(p => p.CouponCode).HasMaxLength(100);
        }
    }
}

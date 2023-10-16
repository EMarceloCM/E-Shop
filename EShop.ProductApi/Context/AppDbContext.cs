using EShop.ProductApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace EShop.ProductApi.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            #region Category
            mb.Entity<Category>().HasKey(k => k.CategoryId);
            mb.Entity<Category>().Property(x => x.Name).HasMaxLength(100).IsRequired();
            #endregion

            #region Product
            mb.Entity<Product>().HasKey(k => k.Id);
            mb.Entity<Product>().Property(x => x.Name).HasMaxLength(100).IsRequired();
            mb.Entity<Product>().Property(x => x.Description).HasMaxLength(255).IsRequired();
            mb.Entity<Product>().Property(x => x.ImageURL).HasMaxLength(255).IsRequired();
            mb.Entity<Product>().Property(x => x.Price).HasPrecision(12, 2);
            #endregion

            mb.Entity<Category>().HasMany(x => x.Products).WithOne(x => x.Category).IsRequired().OnDelete(DeleteBehavior.Cascade);

            #region Seeds
            mb.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "Material Escolar"
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Acessórios"
                }
            );
            #endregion
        }
    }
}

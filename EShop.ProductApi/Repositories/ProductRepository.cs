using EShop.ProductApi.Context;
using EShop.ProductApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.ProductApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.Include(c => c.Category).AsNoTracking().ToListAsync();
        }
        public async Task<Product> GetById(int id)
        {
            return await _context.Products.Include(c => c.Category).Where(p => p.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<Product> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product> Update(Product product)
        {
            _context.Products.Update(product);
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product> DeleteById(int id)
        {
            var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}

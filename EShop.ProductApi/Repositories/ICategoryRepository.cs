using EShop.ProductApi.Models;

namespace EShop.ProductApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<IEnumerable<Category>> GetCategoriesProducts();
        Task<Category> Create(Category category);
        Task<Category> Update(Category category);
        Task<Category> Delete(int id);
    }
}

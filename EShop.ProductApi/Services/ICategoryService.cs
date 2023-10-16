using EShop.ProductApi.DTOs;

namespace EShop.ProductApi.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetCategories();
        Task<IEnumerable<CategoryDTO>> GetCategoriesProducts();
        Task<CategoryDTO> GetCategoryById(int id);
        Task AddCategory(CategoryDTO categoryDTO);
        Task RemoveCategory(int id);
        Task UpdateCategory(CategoryDTO categoryDTO);

    }
}

using EShop.ProductApi.DTOs;

namespace EShop.ProductApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProductById(int id);
        Task AddProduct(ProductDTO productDto);
        Task RemoveProduct(int id);
        Task UpdateProduct(ProductDTO productDto);
    }
}

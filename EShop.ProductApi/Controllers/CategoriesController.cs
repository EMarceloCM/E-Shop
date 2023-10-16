using EShop.ProductApi.DTOs;
using EShop.ProductApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categoriesDto = await _categoryService.GetCategories();

            if (categoriesDto == null)
                return NotFound("Categories Not Found");
            return Ok(categoriesDto);
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesProducts()
        {
            var categoriesDto = await _categoryService.GetCategoriesProducts();

            if (categoriesDto == null)
                return NotFound("Categories Not Found");
            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var categoriesDto = await _categoryService.GetCategoryById(id);
            if (categoriesDto == null)
                return NotFound("Category Not Found");
            return Ok(categoriesDto);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Invalid Data");

            await _categoryService.AddCategory(categoryDto);
            return new CreatedAtRouteResult("GetCategory", new { id = categoryDto.CategoryId }, categoryDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if ((id != categoryDTO.CategoryId) || categoryDTO == null)
                return BadRequest();

            await _categoryService.UpdateCategory(categoryDTO);
            return Ok(categoryDTO);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
        {
            var categoryDto = await _categoryService.GetCategoryById(id);
            if (categoryDto == null)
                return NotFound("Category Not Found");

            await _categoryService.RemoveCategory(id);
            return Ok(categoryDto);
        } 
    }
}

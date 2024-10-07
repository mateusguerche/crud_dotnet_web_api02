using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI_Projeto02.DTOs;
using WebAPI_Projeto02.DTOs.Mappings;
using WebAPI_Projeto02.Filters;
using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;
using WebAPI_Projeto02.Repositories;
using X.PagedList;

namespace WebAPI_Projeto02.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriesController> _logger;
        public CategoriesController(IUnitOfWork uof, ILogger<CategoriesController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters categoriesParameters)
        {
            var categories = await _uof.CategoryRepository.GetCategoriesAsync(categoriesParameters);

            return GetCategoriesPages(categories);
        }

        [HttpGet("filter/name/pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesFilters([FromQuery] CategoriesFilterName categoriesFilter)
        {
            var categoriesFilters = await _uof.CategoryRepository.GetCategoriesFilterNameAsync(categoriesFilter);
            
            return GetCategoriesPages(categoriesFilters);
        }

        private ActionResult<IEnumerable<CategoryDTO>> GetCategoriesPages(IPagedList<Category> categories)
        {
            var metadata = new
            {
                categories.Count,
                categories.PageSize,
                categories.PageCount,
                categories.TotalItemCount,
                categories.HasNextPage,
                categories.HasPreviousPage,
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriesDto = categories.ToCategoryDTOList();

            return Ok(categoriesDto);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categories = await _uof.CategoryRepository.GetAllAsync();

            if (categories is null)
                return NotFound($"Category not found");
   
            var categoriesDto = categories.ToCategoryDTOList();
            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDTO>> Get(int id) 
        {
            var category = await _uof.CategoryRepository.GetAsync(c => c.Id == id);
            if (category is null)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }

            var categoryDto = category.ToCategoryDTO();         
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDto)
        {
            if (categoryDto is null)
            {
                _logger.LogWarning($"Invalid data");
                return BadRequest("Invalid data");
            }
            
            var category = categoryDto.ToCategory();

            var newCategory = _uof.CategoryRepository.Create(category);
            await _uof.CommitAsync();

            var newCategoryDto = newCategory.ToCategoryDTO();

            return new CreatedAtRouteResult("GetCategoryById", new { id = newCategoryDto.Id }, newCategoryDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.Id)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }

            var category = categoryDto.ToCategory();

            var updateCategory = _uof.CategoryRepository.Update(category);
            await _uof.CommitAsync();

            var updateCategoryDto = updateCategory.ToCategoryDTO();

            return Ok(updateCategoryDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
        {
            var category = await _uof.CategoryRepository.GetAsync(c => c.Id == id);

            if (category is null)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }
            
            var removeCategory = _uof.CategoryRepository.Delete(category);
            await _uof.CommitAsync();

            var removeCategoryDto = removeCategory.ToCategoryDTO();

            return Ok(removeCategoryDto);
        }
    }
}

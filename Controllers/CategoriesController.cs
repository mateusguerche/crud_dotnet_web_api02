using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI_Projeto02.DTOs;
using WebAPI_Projeto02.DTOs.Mappings;
using WebAPI_Projeto02.Filters;
using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;
using WebAPI_Projeto02.Repositories;

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
        public ActionResult<IEnumerable<CategoryDTO>> Get([FromQuery] CategoriesParameters categoriesParameters)
        {
            var categories = _uof.CategoryRepository.GetCategories(categoriesParameters);

            return GetCategoriesPages(categories);
        }

        [HttpGet("filter/name/pagination")]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategoriesFilters([FromQuery] CategoriesFilterName categoriesFilter)
        {
            var categoriesFilters = _uof.CategoryRepository.GetCategoriesFilterName(categoriesFilter);
            
            return GetCategoriesPages(categoriesFilters);
        }

        private ActionResult<IEnumerable<CategoryDTO>> GetCategoriesPages(PagedList<Category> categories)
        {
            var metadata = new
            {
                categories.TotalCount,
                categories.PageSize,
                categories.CurrentPage,
                categories.TotalPages,
                categories.HasNext,
                categories.HasPrevious,
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriesDto = categories.ToCategoryDTOList();

            return Ok(categoriesDto);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoryDTO>> Get()
        {
            var categories = _uof.CategoryRepository.GetAll();

            if (categories is null)
                return NotFound($"Category not found");
   
            var categoriesDto = categories.ToCategoryDTOList();
            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public ActionResult<CategoryDTO> Get(int id) 
        {
            var category = _uof.CategoryRepository.Get(c => c.Id == id);
            if (category is null)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }

            var categoryDto = category.ToCategoryDTO();         
            return Ok(categoryDto);
        }

        [HttpPost]
        public ActionResult<CategoryDTO> Post(CategoryDTO categoryDto)
        {
            if (categoryDto is null)
            {
                _logger.LogWarning($"Invalid data");
                return BadRequest("Invalid data");
            }
            
            var category = categoryDto.ToCategory();

            var newCategory = _uof.CategoryRepository.Create(category);
            _uof.Commit();

            var newCategoryDto = newCategory.ToCategoryDTO();

            return new CreatedAtRouteResult("GetCategoryById", new { id = newCategoryDto.Id }, newCategoryDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.Id)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }

            var category = categoryDto.ToCategory();

            var updateCategory = _uof.CategoryRepository.Update(category);
            _uof.Commit();

            var updateCategoryDto = updateCategory.ToCategoryDTO();

            return Ok(updateCategoryDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoryDTO> Delete(int id)
        {
            var category = _uof.CategoryRepository.Get(c => c.Id == id);

            if (category is null)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }
            
            var removeCategory = _uof.CategoryRepository.Delete(category);
            _uof.Commit();

            var removeCategoryDto = removeCategory.ToCategoryDTO();

            return Ok(removeCategoryDto);
        }
    }
}

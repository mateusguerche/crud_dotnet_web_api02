using Microsoft.AspNetCore.Mvc;
using WebAPI_Projeto02.DTOs;
using WebAPI_Projeto02.DTOs.Mappings;
using WebAPI_Projeto02.Filters;
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.Filters;
using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Repositories;

namespace WebAPI_Projeto02.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoriesController> _logger;
        public CategoriesController(ICategoryRepository repository, ILogger<CategoriesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Category>> Get()
        {
            var categories = _repository.GetCategories();                      
            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public ActionResult<Category> Get(int id) 
        {
            var category = _repository.GetCategory(id);
            if (category is null)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }
            return Ok(category);
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
            {
                _logger.LogWarning($"Invalid data");
                return BadRequest("Invalid data");
            }
            var newCategory = _repository.Create(category);

            return new CreatedAtRouteResult("GetCategoryById", new { id = newCategory.Id }, newCategory);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.Id)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }

            _repository.Update(category);
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _repository.GetCategory(id);

            if (category is null)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }
            
            var removeCategoy = _repository.Delete(id);
            return Ok(category);
        }
    }
}

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
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriesController> _logger;
        public CategoriesController(IUnitOfWork uof, ILogger<CategoriesController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Category>> Get()
        {
            var categories = _uof.CategoryRepository.GetAll();                      
            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public ActionResult<Category> Get(int id) 
        {
            var category = _uof.CategoryRepository.Get(c => c.Id == id);
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
            var newCategory = _uof.CategoryRepository.Create(category);
            _uof.Commit();

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

            _uof.CategoryRepository.Update(category);
            _uof.Commit();
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _uof.CategoryRepository.Get(c => c.Id == id);

            if (category is null)
            {
                _logger.LogWarning($"Category not found");
                return NotFound($"Category not found");
            }
            
            var removeCategory = _uof.CategoryRepository.Delete(category);
            _uof.Commit();
            return Ok(removeCategory);
        }
    }
}

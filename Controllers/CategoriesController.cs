using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.Models;

namespace WebAPI_Projeto02.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
        {
            try
            {
                var categories = _context.Categories.AsNoTracking().Include(p => p.Products).ToList();

                if (categories is null)
                    return NotFound("Categories not found!");

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            try
            {
                var categories = _context.Categories.AsNoTracking().ToList();

                if (categories is null)
                    return NotFound("Categories not found!");

                return Ok(categories);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public ActionResult<Category> Get(int id) 
        {
            try { 
                var category = _context.Categories.AsNoTracking().FirstOrDefault(x => x.Id == id);

                if (category is null)
                    return NotFound("Category not found");

                return Ok(category);
            
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            try { 
                if(category is null)
                    return BadRequest();

                _context.Categories.Add(category);
                _context.SaveChanges();
                return new CreatedAtRouteResult("GetCategoryById", new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            try { 
            if (id != category.Id)
                return NotFound("Category not found");

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(category);
        }
            catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try {
                var category = _context.Categories.FirstOrDefault(p => p.Id == id);

            if (category is null)
                return NotFound("Category not found");
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok(category);
            }
            catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }
    }
}

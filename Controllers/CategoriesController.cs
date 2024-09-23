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
        private readonly IConfiguration _configuration;
        public CategoriesController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("ReadSettings")]
        public string GetValue()
        {
            var key1 = _configuration ["key1"];
            var key2 = _configuration ["key2"];

            var section1key2 = _configuration["section1:key2"];

            return $"Key1 = {key1} \nKey2 = {key2} \nSeciotn1 => Key2 = {section1key2}";
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesProducts()
        {
            try
            {
                var categories = await _context.Categories.AsNoTracking().Include(p => p.Products).ToListAsync();

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
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            try
            {
                var categories = await _context.Categories.AsNoTracking().ToListAsync();

                if (categories is null)
                    return NotFound("Categories not found!");

                return Ok(categories);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<Category>> Get(int id) 
        {
            try { 
                var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

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

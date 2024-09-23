using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.Filters;
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
            var categories = await _context.Categories.AsNoTracking().Include(p => p.Products).ToListAsync();
            
            if (categories is null)
                return NotFound("Categories not found!");
            
            return Ok(categories);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
           
            if (categories is null)
                return NotFound("Categories not found!");
            
            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<Category>> Get(int id) 
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            
            if (category is null)
                return NotFound("Category not found");
            
            return Ok(category);
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if(category is null)
                return BadRequest();
            
            _context.Categories.Add(category);
            _context.SaveChanges();
            return new CreatedAtRouteResult("GetCategoryById", new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.Id)
                return NotFound("Category not found");

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(p => p.Id == id);

            if (category is null)
                return NotFound("Category not found");
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok(category);
        }
    }
}

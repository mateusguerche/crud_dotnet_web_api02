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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try {
                var products = _context.Products.AsNoTracking().ToList();

                if (products is null)
                    return NotFound("Products not found!");

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public ActionResult<Product> Get(int id)
        {
            try {
                var product = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == id);

                if (product is null)
                    return NotFound("Product not found");

                return Ok(product);
            }
            catch (Exception ex){
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            try { 
                if (product is null)
                    return BadRequest();

                _context.Products.Add(product);
                _context.SaveChanges();
                return new CreatedAtRouteResult("GetProductById", new { id = product.Id }, product);
            }
            catch (Exception ex){
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            try { 
                if (id != product.Id)
                    return NotFound("Product not found");

                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(product);
            }
            catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try { 
                var product = _context.Products.FirstOrDefault(p => p.Id == id);

                if (product is null)
                    return NotFound("Product not found");
                _context.Products.Remove(product);
                _context.SaveChanges();

                return Ok(product);
            }
            catch (Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }
    }
}

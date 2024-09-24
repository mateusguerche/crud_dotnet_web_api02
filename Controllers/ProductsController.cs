using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Repositories;

namespace WebAPI_Projeto02.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public ProductsController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("products/{id}")]
        public ActionResult<IEnumerable<Product>> GetProductsCategory(int id)
        {
            var products = _uof.ProductRepository.GetProductsByCategory(id);

            if (products is null)
                return NotFound("Products not found!");

            return Ok(products);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _uof.ProductRepository.GetAll();
            
            if (products is null)
                return NotFound("Products not found!");
            
            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public ActionResult<Product> Get(int id)
        {
            var product = _uof.ProductRepository.Get(p  => p.Id == id);
            
            if (product is null)
                return NotFound("Product not found");
            
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
                return BadRequest();
            
            var newProduct = _uof.ProductRepository.Create(product);
            _uof.Commit();
            
            return new CreatedAtRouteResult("GetProductById", new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.Id)
                return NotFound("Product not found");

            _uof.ProductRepository.Update(product);
            _uof.Commit();
            return Ok(product);  
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _uof.ProductRepository.Get(p => p.Id == id);

            if (product is null)  
                return NotFound($"Product not found");

            var removeProduct = _uof.ProductRepository.Delete(product);
            _uof.Commit();
            return Ok(removeProduct);
        }
    }
}

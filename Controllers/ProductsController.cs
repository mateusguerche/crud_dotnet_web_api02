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
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _repository.GetProducts().ToList();
            
            if (products is null)
                return NotFound("Products not found!");
            
            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public ActionResult<Product> Get(int id)
        {
            var product = _repository.GetProduct(id);
            
            if (product is null)
                return NotFound("Product not found");
            
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
                return BadRequest();
            
            var newProduct = _repository.Create(product);
            
            return new CreatedAtRouteResult("GetProductById", new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.Id)
                return NotFound("Product not found");
            
            bool isProductUpdated = _repository.Update(product);
            if (isProductUpdated)
            {
                return Ok(product);
            }
            else
            {
                return StatusCode(500, $"An error occurred while updating the product. Please try again.");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            bool isProductUpdated = _repository.Delete(id);
            if (isProductUpdated)
            {
                return Ok($"Product was successfully deleted.");
            }
            else
            {
                return StatusCode(500, "An error occurred while deleting the product. Please try again.");

            }
        }
    }
}

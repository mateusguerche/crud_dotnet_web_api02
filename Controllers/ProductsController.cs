using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.DTOs;
using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;
using WebAPI_Projeto02.Repositories;

namespace WebAPI_Projeto02.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("products/{id}")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsCategory(int id)
        {
            var products = _uof.ProductRepository.GetProductsByCategory(id);

            if (products is null)
                return NotFound("Products not found!");

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDto);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProductDTO>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = _uof.ProductRepository.GetProducts(productsParameters);
            return GetProductsPages(products);
        }

        [HttpGet("filter/price/pagination")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsFilterPrice([FromQuery] ProductsFilterPrice productsFilterParams)
        {
            var products = _uof.ProductRepository.GetProductsFilterPrice(productsFilterParams);
            return GetProductsPages(products);
        }

        private ActionResult<IEnumerable<ProductDTO>> GetProductsPages(PagedList<Product> products)
        {
            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious,
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {
            var products = _uof.ProductRepository.GetAll();
            
            if (products is null)
                return NotFound("Products not found!");

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDto);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public ActionResult<ProductDTO> Get(int id)
        {
            var product = _uof.ProductRepository.Get(p  => p.Id == id);
            
            if (product is null)
                return NotFound("Product not found");

            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public ActionResult<ProductDTO> Post(ProductDTO productDto)
        {
            if (productDto is null)
                return BadRequest();

            var product = _mapper.Map<Product>(productDto);

            var newProduct = _uof.ProductRepository.Create(product);
            _uof.Commit();

            var newProductDto = _mapper.Map<ProductDTO>(newProduct);

            return new CreatedAtRouteResult("GetProductById", new { id = newProductDto.Id }, newProductDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProductDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
        {
            if (patchProductDTO is null || id <= 0)
                return BadRequest("FFFF");

            var product = _uof.ProductRepository.Get(p => p.Id == id);
            if (product is null)
                return NotFound("esta vazio");

            var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

            patchProductDTO.ApplyTo(productUpdateRequest, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(productUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(productUpdateRequest, product);
            _uof.ProductRepository.Update(product);
            _uof.Commit();

            return Ok(_mapper.Map<ProductDTOUpdateResponse>(product));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProductDTO> Put(int id, ProductDTO productDto)
        {
            if (id != productDto.Id)
                return NotFound("Product not found");

            var product = _mapper.Map<Product>(productDto);

           var updateProduct = _uof.ProductRepository.Update(product);
            _uof.Commit();

            var updateProductDto = _mapper.Map<ProductDTO>(updateProduct);

            return Ok(updateProductDto);  
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProductDTO> Delete(int id)
        {
            var product = _uof.ProductRepository.Get(p => p.Id == id);

            if (product is null)  
                return NotFound($"Product not found");

            var removeProduct = _uof.ProductRepository.Delete(product);
            _uof.Commit();

            var removeProductDto = _mapper.Map<ProductDTO>(removeProduct);

            return Ok(removeProductDto);
        }
    }
}

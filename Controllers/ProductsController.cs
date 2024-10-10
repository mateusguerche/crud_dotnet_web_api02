using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using X.PagedList;

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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsCategory(int id)
        {
            var products = await _uof.ProductRepository.GetProductsByCategoryAsync(id);

            if (products is null)
                return NotFound("Products not found!");

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = await _uof.ProductRepository.GetProductsAsync(productsParameters);
            return GetProductsPages(products);
        }

        [HttpGet("filter/price/pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsFilterPrice([FromQuery] ProductsFilterPrice productsFilterParams)
        {
            var products = await _uof.ProductRepository.GetProductsFilterPriceAsync(productsFilterParams);
            return GetProductsPages(products);
        }

        private ActionResult<IEnumerable<ProductDTO>> GetProductsPages(IPagedList<Product> products)
        {
            var metadata = new
            {
                products.Count,
                products.PageSize,
                products.PageCount,
                products.TotalItemCount,
                products.HasNextPage,
                products.HasPreviousPage,
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await _uof.ProductRepository.GetAllAsync();
            
            if (products is null)
                return NotFound("Products not found!");

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDto);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _uof.ProductRepository.GetAsync(p  => p.Id == id);
            
            if (product is null)
                return NotFound("Product not found");

            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post(ProductDTO productDto)
        {
            if (productDto is null)
                return BadRequest();

            var product = _mapper.Map<Product>(productDto);

            var newProduct = _uof.ProductRepository.Create(product);
            await _uof.CommitAsync();

            var newProductDto = _mapper.Map<ProductDTO>(newProduct);

            return new CreatedAtRouteResult("GetProductById", new { id = newProductDto.Id }, newProductDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public async Task<ActionResult<ProductDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
        {
            if (patchProductDTO is null || id <= 0)
                return BadRequest("FFFF");

            var product = await _uof.ProductRepository.GetAsync(p => p.Id == id);
            if (product is null)
                return NotFound("esta vazio");

            var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

            patchProductDTO.ApplyTo(productUpdateRequest, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(productUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(productUpdateRequest, product);
            _uof.ProductRepository.Update(product);
            await _uof.CommitAsync();

            return Ok(_mapper.Map<ProductDTOUpdateResponse>(product));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO productDto)
        {
            if (id != productDto.Id)
                return NotFound("Product not found");

            var product = _mapper.Map<Product>(productDto);

           var updateProduct = _uof.ProductRepository.Update(product);
            await _uof.CommitAsync();

            var updateProductDto = _mapper.Map<ProductDTO>(updateProduct);

            return Ok(updateProductDto);  
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var product = await _uof.ProductRepository.GetAsync(p => p.Id == id);

            if (product is null)  
                return NotFound($"Product not found");

            var removeProduct = _uof.ProductRepository.Delete(product);
            await _uof.CommitAsync();

            var removeProductDto = _mapper.Map<ProductDTO>(removeProduct);

            return Ok(removeProductDto);
        }
    }
}

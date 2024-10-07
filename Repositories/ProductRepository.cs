using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;
using X.PagedList;

namespace WebAPI_Projeto02.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParams)
        {
            var products = await GetAllAsync();
            var productsOrdered  = products.OrderBy(p => p.Name).AsQueryable();
            //var result = PagedList<Product>.ToPagedList(productsOrdered, productsParams.PageNumber, productsParams.PageSize);
            var result = await productsOrdered.ToPagedListAsync(productsParams.PageNumber, productsParams.PageSize);
            return result;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id)
        {
            var products = await GetAllAsync();
            var productsCategory = products.Where(p => p.CategoryId == id);
            return productsCategory;
        }

        public async Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterParams)
        {
            var products = await GetAllAsync();
            if (productsFilterParams.Price.HasValue && !string.IsNullOrEmpty(productsFilterParams.PriceCriterion))
            {
                if (productsFilterParams.PriceCriterion.Equals("maior", StringComparison.OrdinalIgnoreCase))
                    products = products.Where(p => p.Price > productsFilterParams.Price.Value).OrderBy(p => p.Price);

                else if (productsFilterParams.PriceCriterion.Equals("menor", StringComparison.OrdinalIgnoreCase))
                    products = products.Where(p => p.Price < productsFilterParams.Price.Value).OrderBy(p => p.Price);

                else if (productsFilterParams.PriceCriterion.Equals("igual", StringComparison.OrdinalIgnoreCase))
                    products = products.Where(p => p.Price == productsFilterParams.Price.Value).OrderBy(p => p.Price);
            }

            //var productsFilter = PagedList<Product>.ToPagedList(products.AsQueryable(), productsFilterParams.PageNumber, productsFilterParams.PageSize);
            var productsFilter = await products.ToPagedListAsync(productsFilterParams.PageNumber, productsFilterParams.PageSize);
            return productsFilter;
        }
    }
}

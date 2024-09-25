using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;

namespace WebAPI_Projeto02.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public PagedList<Product> GetProducts(ProductsParameters productsParams)
        {
            var products = GetAll().OrderBy(p => p.Name).AsQueryable();
            var productsOrdered = PagedList<Product>.ToPagedList(products, productsParams.PageNumber, productsParams.PageSize);
            return productsOrdered;
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return GetAll().Where(p => p.CategoryId == id);
        }

        public PagedList<Product> GetProductsFilterPrice(ProductsFilterPrice productsFilterParams)
        {
           var products = GetAll().AsQueryable();
            if (productsFilterParams.Price.HasValue && !string.IsNullOrEmpty(productsFilterParams.PriceCriterion))
            {
                if (productsFilterParams.PriceCriterion.Equals("maior", StringComparison.OrdinalIgnoreCase))
                    products = products.Where(p => p.Price > productsFilterParams.Price.Value).OrderBy(p => p.Price);

                else if (productsFilterParams.PriceCriterion.Equals("menor", StringComparison.OrdinalIgnoreCase))
                    products = products.Where(p => p.Price < productsFilterParams.Price.Value).OrderBy(p => p.Price);

                else if (productsFilterParams.PriceCriterion.Equals("igual", StringComparison.OrdinalIgnoreCase))
                    products = products.Where(p => p.Price == productsFilterParams.Price.Value).OrderBy(p => p.Price);
            }

            var productsFilter = PagedList<Product>.ToPagedList(products, productsFilterParams.PageNumber, productsFilterParams.PageSize);
            return productsFilter;
        }
    }
}

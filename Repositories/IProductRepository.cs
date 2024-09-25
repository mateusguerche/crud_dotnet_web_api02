using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;

namespace WebAPI_Projeto02.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        PagedList<Product> GetProducts(ProductsParameters productsParams);
        PagedList<Product> GetProductsFilterPrice(ProductsFilterPrice productsFilterParams);
        IEnumerable<Product> GetProductsByCategory(int id);
    }
}

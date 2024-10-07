using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;
using X.PagedList;

namespace WebAPI_Projeto02.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParams);
        Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterParams);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);
    }
}

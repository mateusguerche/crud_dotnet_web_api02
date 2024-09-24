using WebAPI_Projeto02.Models;

namespace WebAPI_Projeto02.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsByCategory(int id);
    }
}

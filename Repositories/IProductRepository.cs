using WebAPI_Projeto02.Models;

namespace WebAPI_Projeto02.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        Product GetProduct(int id);
        Product Create(Product product);
        bool Update(Product product);
        bool Delete(int id);
    }
}

using WebAPI_Projeto02.Models;

namespace WebAPI_Projeto02.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);
        Category Create(Category category);
        Category Update(Category category);
        Category Delete(int id);
    }
}

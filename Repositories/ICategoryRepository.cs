using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;

namespace WebAPI_Projeto02.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        PagedList<Category> GetCategories(CategoriesParameters categoriesParams);
        PagedList<Category> GetCategoriesFilterName(CategoriesFilterName categoriesParams);
    }
}

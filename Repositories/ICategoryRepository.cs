using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;
using X.PagedList;

namespace WebAPI_Projeto02.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParams);
        Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesParams);
    }
}

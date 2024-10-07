using Microsoft.EntityFrameworkCore;
using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;
using X.PagedList;

namespace WebAPI_Projeto02.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {}

        public async Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParams)
        {
            var categories = await GetAllAsync();
            var categoriesOrdered = categories.OrderBy(c => c.Name).AsQueryable();
            //var result = PagedList<Category>.ToPagedList(categoriesOrdered, categoriesParams.PageNumber, categoriesParams.PageSize);
            var result = await categoriesOrdered.ToPagedListAsync(categoriesParams.PageNumber, categoriesParams.PageSize);
            return result;
        }

        public async Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesParams)
        {
            var categories = await GetAllAsync();
            if(!string.IsNullOrEmpty(categoriesParams.Name))
                categories = categories.Where(c => c.Name.Contains(categoriesParams.Name));
            //var categoriesFilters = PagedList<Category>.ToPagedList(categories.AsQueryable(), categoriesParams.PageNumber, categoriesParams.PageSize);
            var categoriesFilters = await categories.ToPagedListAsync(categoriesParams.PageNumber, categoriesParams.PageSize);
            return categoriesFilters;

        }
    }
}

using Microsoft.EntityFrameworkCore;
using WebAPI_Projeto02.Context;
using WebAPI_Projeto02.Models;
using WebAPI_Projeto02.Pagination;

namespace WebAPI_Projeto02.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {}

        public PagedList<Category> GetCategories(CategoriesParameters categoriesParams)
        {
            var categories = GetAll().OrderBy(c => c.Name).AsQueryable();
            var categoriesOrdered = PagedList<Category>.ToPagedList(categories, categoriesParams.PageNumber, categoriesParams.PageSize);
            return categoriesOrdered;
        }

        public PagedList<Category> GetCategoriesFilterName(CategoriesFilterName categoriesParams)
        {
            var categories = GetAll().AsQueryable();
            if(!string.IsNullOrEmpty(categoriesParams.Name))
                categories = categories.Where(c => c.Name.Contains(categoriesParams.Name));
            var categoriesFilters = PagedList<Category>.ToPagedList(categories, categoriesParams.PageNumber, categoriesParams.PageSize);
            return categoriesFilters;
        }
    }
}

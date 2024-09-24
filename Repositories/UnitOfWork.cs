using WebAPI_Projeto02.Context;

namespace WebAPI_Projeto02.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository? _productRepo;
        public ICategoryRepository? _categoryRepo;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository
        { 
            get 
            { 
                return _productRepo = _productRepo ?? new ProductRepository(_context); 
            } 
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepo = _categoryRepo ?? new CategoryRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

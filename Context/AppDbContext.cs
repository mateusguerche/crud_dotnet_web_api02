using Microsoft.EntityFrameworkCore;
using WebAPI_Projeto02.Models;

namespace WebAPI_Projeto02.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Product>? Products { get; set; }
    }

}



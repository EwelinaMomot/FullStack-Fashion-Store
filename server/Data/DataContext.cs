using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using server.Models;

namespace server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
            public  DbSet<Product> Products { get; set; }
            public DbSet<Comment> Comments { get; set; }

            public DbSet<ProductCategory> ProductCategories { get; set; }

    }
}

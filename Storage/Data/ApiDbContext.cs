using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace Storage.Data {
    public class ApiDbContext : DbContext {
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderModel> Orders { get; set; }

        public ApiDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseSqlite(@"Data Source=Storage.db");
        
    }
}

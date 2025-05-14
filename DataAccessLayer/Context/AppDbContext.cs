using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Veritabanı bağlantı dizesi
            optionsBuilder.UseSqlServer("Server=DESKTOP-141UAGI;Database=TestOrmDb;Trusted_Connection=True; TrustServerCertificate=True;");
        }
        public DbSet<Product> Products { get; set; }
    }
}

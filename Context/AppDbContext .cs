using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore
{
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<admin_account> admin_account { get; set; }
        public DbSet<admin_log> admin_log { get; set; }
        public DbSet<delivery_services> delivery_services { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<tovary> tovary { get; set; }
        public DbSet<categories> categories { get; set; }
        public DbSet<sizes> sizes { get; set; }
        public DbSet<sklad> sklad { get; set; }
        public DbSet<koshik> koshik { get; set; }
        public DbSet<zamovlennya> zamovlennya { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-C4M6251\\MSSQLSERVER01;Database=BrandClothingStore;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.order_id);

            base.OnModelCreating(modelBuilder);
        }
    }
}

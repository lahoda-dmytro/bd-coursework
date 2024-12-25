using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore
{
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<tovary> tovary { get; set; }
        public DbSet<categories> categories { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<koshik> koshik { get; set; }
        public DbSet<admin_log> admin_log { get; set; }
        public DbSet<admin_account> admin_account { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-C4M6251\\MSSQLSERVER01;Database=BrandClothingStore;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.order_id);

            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Tovar)
                .WithMany()
                .HasForeignKey(oi => oi.item_id);

            modelBuilder.Entity<tovary>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tovary)
                .HasForeignKey(t => t.category_id);
        }
    }



}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // =====================
        // DbSets
        // =====================
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } = null!;
        public DbSet<Seller> Sellers { get; set; } = null!;

        // =====================
        // CONFIG
        // =====================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔴 FIX Identity для SQL Server (ГЛАВНОЕ)
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.Property(r => r.Id).HasMaxLength(450);
                entity.Property(r => r.Name).HasMaxLength(256);
                entity.Property(r => r.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.Id).HasMaxLength(450);
            });

            // =====================
            // RELATIONS
            // =====================
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

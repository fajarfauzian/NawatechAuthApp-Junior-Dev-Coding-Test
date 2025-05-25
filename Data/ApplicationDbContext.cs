using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NawatechAuthApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace NawatechAuthApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));
                
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductCategory>().HasQueryFilter(p => !p.IsDeleted);
            
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Product>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Product>()
                .HasOne(p => p.DeletedBy)
                .WithMany()
                .HasForeignKey(p => p.DeletedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
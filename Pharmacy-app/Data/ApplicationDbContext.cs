using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pharmacy_app.Models;
using System.Security.Cryptography.X509Certificates;
using Pharmacy_app.Areas.Admin.Models;
using System.Reflection.Metadata;


namespace Pharmacy_app.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Medicaments)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired();
        }

        public DbSet<Category>? Category { get; set; }*/
        public DbSet<Medicament>? Medicament { get; set; }
/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Medicaments)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired();
        }

        public DbSet<Category>? Category { get; set; }*/
        public DbSet<Pharmacy_app.Areas.Admin.Models.Pharmacists>? Pharmacists { get; set; }
/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Medicaments)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired();
        }

        public DbSet<Category>? Category { get; set; }*/
        public DbSet<Pharmacy_app.Areas.Admin.Models.Faq>? Faq { get; set; }


    }
}
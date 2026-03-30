using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace KASHOP.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<ProductTranslations> ProductTranslations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandTranslations> BrandTranslations { get; set; }
        public readonly IHttpContextAccessor _HttpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor HttpContextAccessor)
            : base(options)
        {
            _HttpContextAccessor = HttpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole<String>>().ToTable("UserRole");
            modelBuilder.Entity<Category>().
                HasOne(c => c.CreatedBy)
                .WithMany().
                HasForeignKey(c => c.CreatedById).
                OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().
                HasOne(c => c.UpdatedBy)
                .WithMany().
                HasForeignKey(c => c.UpdatedById).
                OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>().
              HasOne(c => c.CreatedBy)
              .WithMany().
              HasForeignKey(c => c.CreatedById).
              OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>().
                HasOne(c => c.UpdatedBy)
                .WithMany().
                HasForeignKey(c => c.UpdatedById).
                OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Brand>()
    .HasOne(c => c.CreatedBy)
    .WithMany()
    .HasForeignKey(c => c.CreatedById)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Brand>()
                .HasOne(c => c.UpdatedBy)
                .WithMany()
                .HasForeignKey(c => c.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {    //انا بدي اياك تجيب التغيرات يلي الها علاقه بالكلاسات يلي ورثة من AuditableEntity يعني الكلاسات يلي فيها CreatedBy,CreatedAt,ModifiedBy,ModifiedAt

            if (_HttpContextAccessor.HttpContext!=null)
            {

                var entries = ChangeTracker.Entries<AuditableEntity>();
                var currentUserId = _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); //هنا المفروض تجيب اليوزر الحالي من خلال التوكن
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property(e => e.CreatedById).CurrentValue = currentUserId;
                        entry.Property(e => e.CreatedOn).CurrentValue = DateTime.UtcNow;

                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property(e => e.UpdatedById).CurrentValue = currentUserId;
                        entry.Property(e => e.UpdatedOn).CurrentValue = DateTime.UtcNow;

                    }

                }
            }
          
            return base.SaveChangesAsync( cancellationToken);
        }
    }
}
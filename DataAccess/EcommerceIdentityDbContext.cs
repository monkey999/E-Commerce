using A_Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace B_DataAccess
{
    public class EcommerceIdentityDbContext : IdentityDbContext<UserIdentity>
    {
        public EcommerceIdentityDbContext(DbContextOptions<EcommerceIdentityDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<UserIdentity> UserIdentities { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<OrderStock> OrderStocks { get; set; }
        public virtual DbSet<StockOnHold> StockOnHolds { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserIdentity>().ToTable("AspNetUsers");

            //builder.Entity<OrderStock>()
            //    .HasKey(x => new { x.StockId, x.OrderId });

            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            builder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("ID");

                entity.Property(e => e.Address1).HasMaxLength(50);

                entity.Property(e => e.Address2).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(50)
                    .HasColumnName("Postal_Code");
            });

            builder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Price).HasColumnType("decimal").HasPrecision(18, 2);

                entity.HasOne(d => d.Category)
                     .WithMany(p => p.Products)
                     .HasForeignKey(d => d.CategoryId)
                     .HasConstraintName("FK_Product_Category");
            });
        }
    }
}

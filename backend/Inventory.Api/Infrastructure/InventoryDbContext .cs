using Inventory.Api.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Inventory.Api.Domain.Entities;

namespace Inventory.Api.Infrastructure
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<RawMaterial> RawMaterials => Set<RawMaterial>();
        public DbSet<BillOfMaterialItem> BillOfMaterialItems => Set<BillOfMaterialItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("products");
                e.HasKey(x => x.Id);

                e.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
                e.Property(x => x.Price).HasColumnName("price").HasPrecision(18, 2).IsRequired();

                e.HasIndex(x => x.Code).IsUnique();
            });

            modelBuilder.Entity<RawMaterial>(e =>
            {
                e.ToTable("raw_materials");
                e.HasKey(x => x.Id);

                e.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
                e.Property(x => x.StockQuantity).HasColumnName("stock_quantity").HasPrecision(18, 3).IsRequired();

                e.HasIndex(x => x.Code).IsUnique();
            });

            modelBuilder.Entity<BillOfMaterialItem>(e =>
            {
                e.ToTable("bill_of_material_items");
                e.HasKey(x => x.Id);

                e.Property(x => x.ProductId).HasColumnName("product_id").IsRequired();
                e.Property(x => x.RawMaterialId).HasColumnName("raw_material_id").IsRequired();
                e.Property(x => x.QuantityPerUnit).HasColumnName("quantity_per_unit").HasPrecision(18, 3).IsRequired();

                e.HasOne(x => x.Product)
                 .WithMany(p => p.BillOfMaterials)
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.RawMaterial)
                 .WithMany(r => r.BillOfMaterials)
                 .HasForeignKey(x => x.RawMaterialId)
                 .OnDelete(DeleteBehavior.Restrict);

                // A product cannot have the same raw material twice in its BOM
                e.HasIndex(x => new { x.ProductId, x.RawMaterialId }).IsUnique();
            });
        }
    }
}

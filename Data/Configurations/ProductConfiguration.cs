using Alwalid.Cms.Api.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Alwalid.Cms.Api.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EnglishName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ArabicName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.EnglishDescription).HasMaxLength(2000);
            entity.Property(e => e.ArabicDescription).HasMaxLength(2000);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Stock).IsRequired();

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(e => e.Department)
                  .WithMany(e => e.Products)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                  .WithMany(e => e.Products)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Currency)
                  .WithMany(e => e.Products)
                  .HasForeignKey(e => e.CurrencyId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Images)
                  .WithOne(e => e.Product)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Statistics)
                  .WithOne(e => e.Product)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

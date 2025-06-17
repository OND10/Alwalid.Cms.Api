using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Alwalid.Cms.Api.Entities;
using System.Reflection.Emit;

namespace Alwalid.Cms.Api.Data.Configurations
{


    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EnglishName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ArabicName).IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.Department)
                  .WithMany()
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Products)
                  .WithOne(e => e.Category)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasQueryFilter(e => !e.IsDeleted);
        }
    }

}
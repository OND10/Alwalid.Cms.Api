using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Data.Configurations
{


    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EnglishName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ArabicName).IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.Branch)
                  .WithMany(e => e.Departments)
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Products)
                  .WithOne(e => e.Department)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
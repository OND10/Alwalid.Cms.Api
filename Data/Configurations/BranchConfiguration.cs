using Alwalid.Cms.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alwalid.Cms.Api.Data.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(500);

            // Many-to-One relationship with Country
            entity.HasOne(e => e.Country)
                  .WithMany(e => e.Branches)
                  .HasForeignKey(e => e.CountryId)
                  .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship with Department
            entity.HasMany(e => e.Departments)
                  .WithOne(e => e.Branch)
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

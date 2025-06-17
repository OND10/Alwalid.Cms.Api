using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Data.Configurations
{


public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Code).IsRequired().HasMaxLength(3);

        entity.HasMany(e => e.Branches)
              .WithOne(e => e.Country)
              .HasForeignKey(e => e.CountryId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}

}
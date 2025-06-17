using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Alwalid.Cms.Api.Entities;


namespace Alwalid.Cms.Api.Data.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(3);
            entity.Property(e => e.Symbol).IsRequired().HasMaxLength(10);

            entity.HasMany(e => e.Products)
                  .WithOne(e => e.Currency)
                  .HasForeignKey(e => e.CurrencyId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }

}

using Alwalid.Cms.Api.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Alwalid.Cms.Api.Data.Configurations
{
    public class ProductStatisticConfiguration : IEntityTypeConfiguration<ProductStatistic>
    {
        public void Configure(EntityTypeBuilder<ProductStatistic> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.QuantitySold).IsRequired();
            entity.Property(e => e.Date).IsRequired();
        }
    }
}

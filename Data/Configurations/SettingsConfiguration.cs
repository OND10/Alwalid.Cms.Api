using Alwalid.Cms.Api.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Alwalid.Cms.Api.Data.Configurations
{
    public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SiteTitle).HasMaxLength(200);
            entity.Property(e => e.SiteSubtitle).HasMaxLength(200);
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
            entity.Property(e => e.FaviconUrl).HasMaxLength(500);
            entity.Property(e => e.SupportEmail).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Facebook).HasMaxLength(200);
            entity.Property(e => e.LinkedIn).HasMaxLength(200);
            entity.Property(e => e.Instagram).HasMaxLength(200);
            entity.Property(e => e.Twitter).HasMaxLength(200);
            entity.Property(e => e.Youtube).HasMaxLength(200);
            entity.Property(e => e.Tiktok).HasMaxLength(200);
            entity.Property(e => e.Copyright).HasMaxLength(200);
            entity.Property(e => e.DefaultLanguage).HasMaxLength(2);
            entity.Property(e => e.DefaultCurrencyCode).HasMaxLength(3);
            entity.Property(e => e.LastUpdated).IsRequired();
        }
    }
}

namespace Alwalid.Cms.Api.Features.Settings.Dtos
{
    public class SettingsResponseDto
    {
        public int Id { get; set; }
        public string SiteTitle { get; set; } = string.Empty;
        public string SiteSubtitle { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string FaviconUrl { get; set; } = string.Empty;
        public string SupportEmail { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Facebook { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string Twitter { get; set; } = string.Empty;
        public string Youtube { get; set; } = string.Empty;
        public string Tiktok { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public bool IsMaintenanceMode { get; set; }
        public string DefaultLanguage { get; set; } = string.Empty;
        public string DefaultCurrencyCode { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
    }
} 
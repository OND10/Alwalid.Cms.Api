namespace Alwalid.Cms.Api.Entities
{
    public class Settings
    {
        public int Id { get; set; }

        // Branding
        public string SiteTitle { get; set; }
        public string SiteSubtitle { get; set; }
        public string LogoUrl { get; set; }
        public string FaviconUrl { get; set; }

        // Contact
        public string SupportEmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // Social Media
        public string Facebook { get; set; }
        public string LinkedIn { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Youtube { get; set; }
        public string Tiktok { get; set; }

        // Footer
        public string Copyright { get; set; }

        // General Config
        public bool IsMaintenanceMode { get; set; }
        public string DefaultLanguage { get; set; }    // "en", "ar"
        public string DefaultCurrencyCode { get; set; } // e.g., "USD"

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    }
}

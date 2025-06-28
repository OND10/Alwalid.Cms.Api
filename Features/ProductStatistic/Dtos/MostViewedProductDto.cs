namespace Alwalid.Cms.Api.Features.ProductStatistic.Dtos
{
    public class MostViewedProductDto
    {
        public int Top { get; set; } = 10;
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-30);
        public DateTime EndDate { get; set; } = DateTime.Today;
    }
} 
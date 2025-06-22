namespace Alwalid.Cms.Api.Features.ProductStatistic.Dtos
{
    public class TopSellingProductDto
    {
        public int Top { get; set; } = 10;
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }

}

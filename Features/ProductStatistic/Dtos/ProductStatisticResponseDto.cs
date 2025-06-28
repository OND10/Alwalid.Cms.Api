namespace Alwalid.Cms.Api.Features.ProductStatistic.Dtos
{
    public class ProductStatisticResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ViewedCounts { get; set; }
        public int QuantitySold { get; set; }
        public DateTime Date { get; set; }
    }
} 
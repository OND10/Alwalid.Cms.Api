namespace Alwalid.Cms.Api.Features.ProductStatistic.Dtos
{
    public class ProductStatisticRequestDto
    {
        public int ProductId { get; set; }
        public int QuantitySold { get; set; }
        public DateTime Date { get; set; }
    }
} 
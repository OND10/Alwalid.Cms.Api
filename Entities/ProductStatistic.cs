namespace Alwalid.Cms.Api.Entities
{
    public class ProductStatistic
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int QuantitySold { get; set; }
        public DateTime Date { get; set; }
    }
}

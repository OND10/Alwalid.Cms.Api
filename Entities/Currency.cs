namespace Alwalid.Cms.Api.Entities
{
    public class Currency
    {
        public int Id { get; set; }

        public string Name { get; set; }         // "US Dollar", "Yemeni Rial"
        public string Code { get; set; }         // "USD", "YER", "ETB"
        public string Symbol { get; set; }       // "$", "﷼", "Br"

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

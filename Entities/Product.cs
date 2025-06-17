using Alwalid.Cms.Api.Shared;

namespace Alwalid.Cms.Api.Entities
{
    public class Product : AuditableEntity
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public string EnglishDescription { get; set; }
        public string ArabicDescription { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? CurrencyId { get; set; }
        public Currency? Currency { get; set; }

        public ICollection<ProductImage> Images { get; set; }
        public ICollection<ProductStatistic> Statistics { get; set; }
    }
}

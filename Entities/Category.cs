using Alwalid.Cms.Api.Features.Category.Events;
using Alwalid.Cms.Api.Shared;

namespace Alwalid.Cms.Api.Entities
{
    public class Category : BaseEntity 
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Product> Products { get; set; }

        public string MarketName { get; set; }

        private Category() { }

        public Category(string EnglishName, string ArabicName, int DepartmentId, string MarketName)
        {
            EnglishName = EnglishName;
            ArabicName = ArabicName;
            MarketName = MarketName;
            DepartmentId = DepartmentId;

            // raise the domain event at creation time
            AddDomainEvent(new CategoryCreatedEvent(this));
        }
    }
}

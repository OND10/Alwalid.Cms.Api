using Alwalid.Cms.Api.Shared;

namespace Alwalid.Cms.Api.Entities
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

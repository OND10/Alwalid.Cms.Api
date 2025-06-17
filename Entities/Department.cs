namespace Alwalid.Cms.Api.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

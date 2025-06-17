namespace Alwalid.Cms.Api.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}

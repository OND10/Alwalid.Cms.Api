namespace Alwalid.Cms.Api.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g. "Yemen", "Somaliland", "Ethiopia"
        public string Code { get; set; } // e.g. "YE", "SO", "ET"

        public ICollection<Branch> Branches { get; set; }
    }
}

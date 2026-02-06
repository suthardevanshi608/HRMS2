using HRMS.Domain.Entities;

namespace HRMS.Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string CreatedByUserId { get; set; }
        public ApplicationUser CreatedByUser { get; set; }
    }
}

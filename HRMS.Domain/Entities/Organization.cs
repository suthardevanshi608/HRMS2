using System;
using HRMS.Domain.Entities;

namespace HRMS.Domain.Entities
{
    public class Organization
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Add these two properly
        public string CreatedBy { get; set; } = string.Empty; // ensure default value
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
    }
}

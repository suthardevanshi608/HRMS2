using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HRMS.Application.DTOs.HR
{
    public class UpdateHRDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

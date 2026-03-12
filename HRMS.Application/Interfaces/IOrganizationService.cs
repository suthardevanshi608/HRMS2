using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HRMS.Application.DTOs.Organization;

namespace HRMS.Application.Interfaces
{
    public interface IOrganizationService
    {
        Task CreateAsync(CreateOrganizationDto dto, string userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Application.DTOs.HR;

namespace HRMS.Application.Interfaces
{
    public interface IHRService
    {
        Task CreateHRAsync(CreateHRDto dto);
    }
}

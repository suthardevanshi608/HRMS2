using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HRMS.Application.DTOs.Employee;

namespace HRMS.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<object> CreateEmployeeAsync(CreateEmployeeDto dto, string currentUserId);
        Task<List<object>> GetEmployeesAsync(string currentUserId, bool isOrganizationAdmin);
        Task<bool> UpdateEmployeeAsync(string id, UpdateEmployeeDto dto, string currentUserId);
        Task<bool> DeleteEmployeeAsync(string id, string currentUserId);
    }
}

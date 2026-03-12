using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> CreateEmployeeAsync(CreateEmployeeDto dto, string currentUserId)
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                CreatedById = currentUserId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return new
            {
                Message = "Employee created successfully",
                employee.Id
            };
        }

        public async Task<List<object>> GetEmployeesAsync(string currentUserId, bool isOrganizationAdmin)
        {
            var query = _context.Employees
                .Include(e => e.CreatedBy)
                .AsQueryable();

            if (!isOrganizationAdmin)
            {
                query = query.Where(e => e.CreatedById == currentUserId);
            }

            var employees = await query.ToListAsync();

            return employees.Select(e => new
            {
                e.Id,
                e.FirstName,
                e.LastName,
                e.Email,
                e.PhoneNumber,
                e.Address,
                CreatedBy = e.CreatedBy != null ? e.CreatedBy.Email : null
            }).Cast<object>().ToList();
        }

        public async Task<bool> UpdateEmployeeAsync(string id, UpdateEmployeeDto dto, string currentUserId)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            if (employee.CreatedById != currentUserId)
                return false;

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.Address = dto.Address;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(string id, string currentUserId)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            if (employee.CreatedById != currentUserId)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

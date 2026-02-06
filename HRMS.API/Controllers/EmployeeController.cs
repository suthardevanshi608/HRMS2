using HRMS.Application.DTOs.Employee;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/employee")]
    [Authorize] // All authenticated users can access, role checks inside
    public class EmployeeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmployeeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // CREATE Employee (Only HR can create employees)
        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                CreatedById = currentUserId!
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee created successfully", employee.Id });
        }

        // GET all employees
        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetEmployees()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var employees = _context.Employees.AsQueryable();

            if (!isAdmin)
            {
                // HR can only see their own employees
                employees = employees.Where(e => e.CreatedById == currentUserId);
            }

            var result = await employees.Select(e => new
            {
                e.Id,
                e.FirstName,
                e.LastName,
                e.Email,
                e.PhoneNumber,
                e.Address,
                CreatedBy = e.CreatedBy.Email
            }).ToListAsync();

            return Ok(result);
        }

        // UPDATE Employee
        [HttpPut("{id}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] UpdateEmployeeDto dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound("Employee not found");

            // HR can only update their own employees
            if (employee.CreatedById != currentUserId)
                return Forbid();

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.Address = dto.Address;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee updated successfully" });
        }

        // DELETE Employee
        [HttpDelete("{id}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound("Employee not found");

            // HR can only delete their own employees
            if (employee.CreatedById != currentUserId)
                return Forbid();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee deleted successfully" });
        }
    }
}

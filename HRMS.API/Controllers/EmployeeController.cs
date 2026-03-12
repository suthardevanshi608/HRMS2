using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/employee")]
    [Authorize(Roles = "OrganizationAdmin,HR")]
    public class EmployeeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(
            UserManager<ApplicationUser> userManager,
            IEmployeeService employeeService)
        {
            _userManager = userManager;
            _employeeService = employeeService;
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _employeeService.CreateEmployeeAsync(dto, currentUserId!);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("OrganizationAdmin");

            var result = await _employeeService.GetEmployeesAsync(currentUserId!, isAdmin);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] UpdateEmployeeDto dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _employeeService.UpdateEmployeeAsync(id, dto, currentUserId!);

            if (!success) return BadRequest("Update failed");

            return Ok(new { message = "Employee updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _employeeService.DeleteEmployeeAsync(id, currentUserId!);

            if (!success) return BadRequest("Delete failed");

            return Ok(new { message = "Employee deleted successfully" });
        }
    }
}

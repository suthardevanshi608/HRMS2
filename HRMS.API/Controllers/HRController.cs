using HRMS.Application.DTOs.HR;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/hr")]
    [Authorize(Roles = "OrganizationAdmin")] // Only Admin can manage HR
    public class HRController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HRController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // CREATE HR
        [HttpPost]
        public async Task<IActionResult> CreateHR([FromBody] CreateHRDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return BadRequest("HR already exists");

            var hrUser = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(hrUser, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(hrUser, "HR");

            return Ok(new { message = "HR created successfully", hrUser.Id });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllHR()
        {
            var allUsers = _context.Users.ToList(); // Get all users from DB
            var hrUsers = new List<object>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "HR"))
                {
                    hrUsers.Add(new { user.Id, user.Email });
                }
            }

            return Ok(hrUsers);
        }


        // UPDATE HR
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHR(string id, [FromBody] UpdateHRDto dto)
        {
            var hrUser = await _userManager.FindByIdAsync(id);
            if (hrUser == null) return NotFound("HR not found");

            hrUser.Email = dto.Email;
            hrUser.UserName = dto.Email;

            var token = await _userManager.GeneratePasswordResetTokenAsync(hrUser);
            var result = await _userManager.ResetPasswordAsync(hrUser, token, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.UpdateAsync(hrUser);

            return Ok(new { message = "HR updated successfully" });
        }

        // DELETE HR
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHR(string id)
        {
            var hrUser = await _userManager.FindByIdAsync(id);
            if (hrUser == null) return NotFound("HR not found");

            var result = await _userManager.DeleteAsync(hrUser);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "HR deleted successfully" });
        }
    }
}

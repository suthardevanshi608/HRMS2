using HRMS.Application.DTOs.Organization;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/organization")]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // CREATE Organization
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrganizationRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            var organization = new Organization
            {
                Name = request.Name,
                Address = request.Address,
                CreatedByUserId = user.Id
            };

            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();

            // Assign Admin role if not assigned
            if (!await _userManager.IsInRoleAsync(user, "OrganizationAdmin"))
            {
                await _userManager.AddToRoleAsync(user, "OrganizationAdmin");
            }

            return Ok(new
            {
                message = "Organization created successfully",
                organization.Id
            });
        }

        // READ - Get all Organizations
        [HttpGet]
        public IActionResult GetAll()
        {
            var organizations = _context.Organizations.ToList();
            return Ok(organizations);
        }


        // UPDATE Organization
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrganizationRequest request)
        {
            var organization = _context.Organizations.FirstOrDefault(x => x.Id == id);
            if (organization == null)
                return NotFound("Organization not found");

            organization.Name = request.Name;
            organization.Address = request.Address;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Organization updated successfully" });
        }

        // DELETE Organization
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var organization = _context.Organizations.FirstOrDefault(x => x.Id == id);
            if (organization == null)
                return NotFound("Organization not found");

            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Organization deleted successfully" });
        }
    }
}

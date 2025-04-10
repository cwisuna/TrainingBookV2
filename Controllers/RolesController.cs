using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingBookV2.Data;
using TrainingBookV2.Dtos;
using TrainingBookV2.Models;

namespace TrainingBookV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RolesController(AppDbContext context, RoleManager<Role> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            var roles = await dbContext.Roles.Select(r => new RoleDto
            {
                RoleID = r.Id,
                RoleName = r.Name,
            }).ToListAsync();

            return Ok(roles);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRoleById(int id)
        {
            var role = await dbContext.Roles
                .Where(r => r.Id == id)
                .Select(r => new RoleDto
                {
                    RoleID = r.Id,
                    RoleName = r.Name,
                })
                .FirstOrDefaultAsync();

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, UpdateRoleDto dto)
        {
            var role = await dbContext.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            role.Name = dto.RoleName;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleDto dto)
        {
            var role = new Role
            {
                Name = dto.RoleName,
            };

            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roleDto = new RoleDto
            {
                RoleID = role.Id,
                RoleName = role.Name
            };

            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, roleDto);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await dbContext.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            dbContext.Roles.Remove(role);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{userId}/assign-role")]
        public async Task<IActionResult> AssignRoleToUser(string userId, [FromBody] AssignRoleDto dto)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var roleExists = await roleManager.RoleExistsAsync(dto.RoleName);
            if (!roleExists)
                return BadRequest("Role does not exist.");

            var result = await userManager.AddToRoleAsync(user, dto.RoleName);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Role assigned successfully.");
        }


        private bool RoleExists(int id)
        {
            return dbContext.Roles.Any(e => e.Id == id);
        }
    }
}

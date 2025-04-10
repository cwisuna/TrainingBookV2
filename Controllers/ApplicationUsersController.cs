using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingBookV2.Data;
using TrainingBookV2.Dtos;
using TrainingBookV2.Models;

namespace TrainingBookV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public ApplicationUsersController(AppDbContext context)
        {
            dbContext = context;
        }

        // GET: api/ApplicationUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUserDto>>> GetAllUsers()
        {
            var users = await dbContext.Users
                .Select(u => new ApplicationUserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    RoleID = u.RoleID,
                    RoleName = u.Role.Name,
                    DepartmentID = u.DepartmentID,
                    DepartmentName = u.Department.DepartmentName
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/ApplicationUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserDto>> GetUserById(int id)
        {
            var user = await dbContext.Users.Where(u => u.Id == id)
                .Select(u => new ApplicationUserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    RoleID = u.RoleID,
                    RoleName = u.Role.Name,
                    DepartmentID = u.DepartmentID,
                    DepartmentName = u.Department.DepartmentName
                })
                .FirstOrDefaultAsync();

            if(user == null)
            {
                NotFound();
            }

            return Ok(user);    
        }

        // PUT: api/ApplicationUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            dbContext.Entry(applicationUser).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
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

        // DELETE: api/ApplicationUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var applicationUser = await dbContext.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            dbContext.Users.Remove(applicationUser);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        //[HttpPut("{userId}/assign-department/{departmentId}")]  PUTTING ON HOLD FOR NOW 
        //public async Task<IActionResult> AssignUserToDepartment(int userId, int departmentId)
        //{
        //    var user = await dbContext.Users.FindAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var department = await dbContext.Departments.FindAsync(departmentId);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    user.DepartmentID = departmentId;

        //    return NoContent();
        //}

        private bool ApplicationUserExists(int id)
        {
            return dbContext.Users.Any(e => e.Id == id);
        }
    }
}

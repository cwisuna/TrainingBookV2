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
            //get all the users and their departments from the db and map them to the ApplicationUserDto
            var users = await dbContext.Users
                .Select(u => new ApplicationUserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    DepartmentID = u.DepartmentID,
                    DepartmentName = u.Department.DepartmentName
                })
                .ToListAsync();

            //return the list of users 
            return Ok(users);
        }

        // GET: api/ApplicationUsers/5
        [HttpGet("by-id/{id}")]
        public async Task<ActionResult<ApplicationUserDto>> GetUserById(int id)
        {
            //get the single user and their department(s) from the db using their userId and map them to the ApplicationUserDto
            var user = await dbContext.Users.Where(u => u.Id == id)
                .Select(u => new ApplicationUserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    DepartmentID = u.DepartmentID,
                    DepartmentName = u.Department.DepartmentName
                })
                .FirstOrDefaultAsync();

            //if user doesnt exist throw an error
            if(user == null)
            {
                NotFound();
            }

            //return the user
            return Ok(user);    
        }

        [HttpGet("by-department/{departmentId}")]
        public async Task<ActionResult<ApplicationUserDto>>GetUsersByDepartmentId(int departmentId)
        {
            //get all the users for a department from the db using their departmentId and map them to the ApplicationUserDto
            var users = await dbContext.Users
                .Where(u => u.DepartmentID == departmentId)
                .Select(u => new ApplicationUserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    DepartmentID = u.DepartmentID,
                    DepartmentName = u.Department.DepartmentName
                })
                .ToListAsync();
            if (users == null || !users.Any())
            {
                return NotFound();
            }

            //return the list of users for that department
            return Ok(users);
        }

        // PUT: api/ApplicationUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateApplicationUserDto dto)
        {
            //getting the user from the db using their userId
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //updating the ApplicationUser properties with the data passed in from the dto
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;

            //marking the user as modified so that the dbContext knows to update it
            dbContext.Entry(user).State = EntityState.Modified;
            try
            {
                //saving the user to the db
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
            //getting the user from the db using their userId
            var applicationUser = await dbContext.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            dbContext.Users.Remove(applicationUser);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{userId}/assign-department/{departmentId}")]
        public async Task<IActionResult> AssignUserToDepartment(int userId, int departmentId)
        {
            //getting the user from the db using their userId
            var user = await dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            //getting the department from the db using the departmentId
            var department = await dbContext.Departments.FindAsync(departmentId);
            if (department == null)
            {
                return NotFound();
            }

            //assigning a user to their department by the departmentId
            user.DepartmentID = departmentId;

            await dbContext.SaveChangesAsync(); 

            return NoContent();
        }

        private bool ApplicationUserExists(int id)
        {
            return dbContext.Users.Any(e => e.Id == id);
        }
    }
}

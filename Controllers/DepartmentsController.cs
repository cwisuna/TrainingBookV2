using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingBookV2.Data;
using TrainingBookV2.Dtos;
using TrainingBookV2.Models;

namespace TrainingBookV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public DepartmentsController(AppDbContext context)
        {
            dbContext = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
        {
            //gets all the departments from the db and maps them to the DepartmentDto
            var departments = await dbContext.Departments.Select(d => new DepartmentDto
            {
                DepartmentID = d.DepartmentID,
                DepartmentName = d.DepartmentName,
            }).ToListAsync();

            //returns the list of departments
            return Ok(departments);
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int id)
        {
            //gets the single department from the db using their departmentId and maps them to the DepartmentDto
            var department = await dbContext.Departments
                .Where(d => d.DepartmentID == id)
                .Select(d => new DepartmentDto
                {
                    DepartmentID = d.DepartmentID,
                    DepartmentName = d.DepartmentName,
                })
                .FirstOrDefaultAsync();

            if (department == null)
            {
                return NotFound();
            }

            //returns the department
            return Ok(department);
        }

        [HttpGet("by-userId{userId}")]
        public async Task<ActionResult<DepartmentDto>> GetUserDepartmentByUserId(int userId)
        {
            var department = await dbContext.Departments
                .Where(d => d.Users.Any(u => u.Id == userId))
                .Select(d => new DepartmentDto
                {
                    DepartmentID = d.DepartmentID,
                    DepartmentName = d.DepartmentName,
                })
                .FirstOrDefaultAsync();
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [Authorize(Roles = "Trainee")]
        [HttpGet("my-department")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentForTrainee()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var department = await dbContext.Departments
                .Where(d => d.Users.Any(u => u.Id == userId))
                .Select(d => new DepartmentDto
                {
                    DepartmentID = d.DepartmentID,
                    DepartmentName = d.DepartmentName,
                })
                .FirstOrDefaultAsync();

            if (department == null)
                return NotFound();

            return Ok(department);
        }


        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, UpdateDepartmentDto dto)
        {
            var department = await dbContext.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            department.DepartmentName = dto.DepartmentName;
            
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentDto dto)
        {
            //creating a department object using the data from the dto
            var department = new Department
            {
                DepartmentName = dto.DepartmentName
            };

            //adding the department to the db
            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();

            //creating a response DTO with the generated DepartmentID and name
            var resultDto = new DepartmentDto
            {
                DepartmentID = department.DepartmentID,
                DepartmentName = department.DepartmentName
            };

            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.DepartmentID }, resultDto);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            //getting the department from the db using their departmentId
            var department = await dbContext.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            dbContext.Departments.Remove(department);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return dbContext.Departments.Any(e => e.DepartmentID == id);
        }
    }
}

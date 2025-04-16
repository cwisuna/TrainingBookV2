using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingBookV2.Data;
using TrainingBookV2.Dtos;
using TrainingBookV2.Models;

namespace TrainingBookV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTrainingBooksController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public UserTrainingBooksController(AppDbContext context)
        {
            dbContext = context;
        }

        // GET: api/UserTrainingBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTrainingBook>>> GetAllUserTrainingBooks()
        {
            return await dbContext.UserTrainingBooks.ToListAsync();
        }

        // GET: api/UserTrainingBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTrainingBook>> GetUserTrainingBookById(int id)
        {
            var userTrainingBook = await dbContext.UserTrainingBooks.FindAsync(id);

            if (userTrainingBook == null)
            {
                return NotFound();
            }

            return userTrainingBook;
        }

        // PUT: api/UserTrainingBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserTrainingBook(int id, UserTrainingBook userTrainingBook)
        {
            if (id != userTrainingBook.UserTrainingBookID)
            {
                return BadRequest();
            }

            dbContext.Entry(userTrainingBook).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTrainingBookExists(id))
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

        // POST: api/UserTrainingBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTrainingBook>> CreateUserTrainingBook(UserTrainingBook userTrainingBook)
        {
            dbContext.UserTrainingBooks.Add(userTrainingBook);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetUserTrainingBook", new { id = userTrainingBook.UserTrainingBookID }, userTrainingBook);
        }

        // DELETE: api/UserTrainingBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTrainingBook(int id)
        {
            var userTrainingBook = await dbContext.UserTrainingBooks.FindAsync(id);
            if (userTrainingBook == null)
            {
                return NotFound();
            }

            dbContext.UserTrainingBooks.Remove(userTrainingBook);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("assign-steps-to-trainingbook")]
        public async Task<ActionResult> AssignStepsToTrainingBook(AssignStepsToTrainingBookDto dto)
        {
            var book = await dbContext.UserTrainingBooks
        .Include(b => b.TrainingSteps)
        .FirstOrDefaultAsync(b => b.UserTrainingBookID == dto.BookId);

            if (book == null)
                return NotFound($"TrainingBook with ID {dto.BookId} not found.");

            var validStepIds = await dbContext.TrainingSteps
                .Where(ts => dto.TrainingStepIds.Contains(ts.StepID))
                .Select(ts => ts.StepID)
                .ToListAsync();

            var existingStepIds = book.TrainingSteps.Select(ts => ts.StepID).ToHashSet();

            var newUserTrainingSteps = validStepIds
                .Where(stepId => !existingStepIds.Contains(stepId))
                .Select(stepId => new UserTrainingStep
                {
                    UserTrainingBookID = dto.BookId,
                    StepID = stepId,
                    IsCompleted = false
                });

            dbContext.UserTrainingStep.AddRange(newUserTrainingSteps);
            await dbContext.SaveChangesAsync();

            return Ok("Training steps successfully assigned to the training book.");
        }

        private bool UserTrainingBookExists(int id)
        {
            return dbContext.UserTrainingBooks.Any(e => e.UserTrainingBookID == id);
        }
    }
}

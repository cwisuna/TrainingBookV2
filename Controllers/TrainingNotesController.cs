using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingBookV2.Data;
using TrainingBookV2.Dtos;
using TrainingBookV2.Models;

namespace TrainingBookV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingNotesController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public TrainingNotesController(AppDbContext context)
        {
            dbContext = context;
        }

        // GET: api/TrainingNotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingNoteDto>>> GetAllTrainingNotes()
        {
            var trainingNotes = await dbContext.TrainingNotes.Select(tn => new TrainingNoteDto
            {
                TrainingNoteId = tn.TrainingNoteID,
                UserTrainingStepId = tn.UserTrainingStepID,
                AuthorId = tn.AuthorID,
                Note = tn.Note,
                CreatedAt = tn.CreatedAt
            }).ToListAsync();

            return Ok(trainingNotes);
        }

        // GET: api/TrainingNotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingNoteDto>> GetTrainingNoteById(int id)
        {
            var trainingNote = await dbContext.TrainingNotes
                .Where(tn => tn.TrainingNoteID == id)
                .Select(tn => new TrainingNoteDto
                {
                    TrainingNoteId = tn.TrainingNoteID,
                    UserTrainingStepId = tn.UserTrainingStepID,
                    AuthorId = tn.AuthorID,
                    Note = tn.Note,
                    CreatedAt = tn.CreatedAt
                })
                .FirstOrDefaultAsync();

            if(trainingNote == null)
            {
                return NotFound();
            }

            return Ok(trainingNote);
        }

        // PUT: api/TrainingNotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingNote(int id, TrainingNote trainingNote)
        {
            if (id != trainingNote.TrainingNoteID)
            {
                return BadRequest();
            }

            dbContext.Entry(trainingNote).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingNoteExists(id))
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

        // POST: api/TrainingNotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainingNoteDto>> CreateTrainingNote(CreateTrainingNoteDto dto)
        {
            var trainingNote = new TrainingNote
            {
                UserTrainingStepID = dto.UserTrainingStepId,
                AuthorID = dto.AuthorId,
                Note = dto.Note,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.TrainingNotes.Add(trainingNote);
            await dbContext.SaveChangesAsync();

            var resultDto = new TrainingNoteDto
            {
                TrainingNoteId = trainingNote.TrainingNoteID,
                UserTrainingStepId = trainingNote.UserTrainingStepID,
                AuthorId = trainingNote.AuthorID,
                Note = trainingNote.Note,
                CreatedAt = trainingNote.CreatedAt
            };

            return CreatedAtAction(nameof(GetTrainingNoteById), new { id = trainingNote.TrainingNoteID }, resultDto);
        }

        // DELETE: api/TrainingNotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingNote(int id)
        {
            var trainingNote = await dbContext.TrainingNotes.FindAsync(id);
            if (trainingNote == null)
            {
                return NotFound();
            }

            dbContext.TrainingNotes.Remove(trainingNote);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingNoteExists(int id)
        {
            return dbContext.TrainingNotes.Any(e => e.TrainingNoteID == id);
        }
    }
}

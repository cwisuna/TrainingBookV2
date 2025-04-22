using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingBookV2.Data;
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
        public async Task<ActionResult<IEnumerable<TrainingNote>>> GetTrainingNotes()
        {
            return await dbContext.TrainingNotes.ToListAsync();
        }

        // GET: api/TrainingNotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingNote>> GetTrainingNote(int id)
        {
            var trainingNote = await dbContext.TrainingNotes.FindAsync(id);

            if (trainingNote == null)
            {
                return NotFound();
            }

            return trainingNote;
        }

        // PUT: api/TrainingNotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingNote(int id, TrainingNote trainingNote)
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
        public async Task<ActionResult<TrainingNote>> PostTrainingNote(TrainingNote trainingNote)
        {
            dbContext.TrainingNotes.Add(trainingNote);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetTrainingNote", new { id = trainingNote.TrainingNoteID }, trainingNote);
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

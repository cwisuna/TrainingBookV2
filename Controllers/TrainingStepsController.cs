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
    public class TrainingStepsController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public TrainingStepsController(AppDbContext context)
        {
            dbContext = context;
        }

        // GET: api/TrainingSteps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingStep>>> GetTrainingSteps()
        {
            return await dbContext.TrainingSteps.ToListAsync();
        }

        // GET: api/TrainingSteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingStep>> GetTrainingStep(int id)
        {
            var trainingStep = await dbContext.TrainingSteps.FindAsync(id);

            if (trainingStep == null)
            {
                return NotFound();
            }

            return trainingStep;
        }

        // PUT: api/TrainingSteps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingStep(int id, TrainingStep trainingStep)
        {
            if (id != trainingStep.StepID)
            {
                return BadRequest();
            }

            dbContext.Entry(trainingStep).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingStepExists(id))
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

        // POST: api/TrainingSteps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainingStep>> PostTrainingStep(TrainingStep trainingStep)
        {
            dbContext.TrainingSteps.Add(trainingStep);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetTrainingStep", new { id = trainingStep.StepID }, trainingStep);
        }

        // DELETE: api/TrainingSteps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingStep(int id)
        {
            var trainingStep = await dbContext.TrainingSteps.FindAsync(id);
            if (trainingStep == null)
            {
                return NotFound();
            }

            dbContext.TrainingSteps.Remove(trainingStep);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingStepExists(int id)
        {
            return dbContext.TrainingSteps.Any(e => e.StepID == id);
        }
    }
}

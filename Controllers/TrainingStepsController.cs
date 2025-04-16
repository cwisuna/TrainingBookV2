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
    public class TrainingStepsController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public TrainingStepsController(AppDbContext context)
        {
            dbContext = context;
        }

        // GET: api/TrainingSteps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingStepsDto>>> GetAllTrainingSteps()
        {
            var trainingSteps = await dbContext.TrainingSteps
                .Select(ts => new TrainingStepsDto
                {
                    Step = ts.Step,
                    Item = ts.Item,
                    Description = ts.Description,
                    TraineeExpectation = ts.TraineeExpectation,
                    TrainerExpectation = ts.TrainerExpectation,
                    TrainingDuration = ts.TrainingDuration,
                    FilePath = ts.FilePath,
                    IsCompleted = ts.IsCompleted,
                    IsSignedOff = ts.IsSignedOff,
                    LastModifiedBy = ts.LastModifiedBy
                })
                .ToListAsync();
            return Ok(trainingSteps);
        }

        // GET: api/TrainingSteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingStepsDto>> GetTrainingStepById(int id)
        {
            var trainingStep = await dbContext.TrainingSteps.Where(ts => ts.StepID == id)
                .Select(ts => new TrainingStepsDto
                {
                    Step = ts.Step,
                    Item = ts.Item,
                    Description = ts.Description,
                    TraineeExpectation = ts.TraineeExpectation,
                    TrainerExpectation = ts.TrainerExpectation,
                    TrainingDuration = ts.TrainingDuration,
                    FilePath = ts.FilePath,
                    IsCompleted = ts.IsCompleted,
                    IsSignedOff = ts.IsSignedOff,
                    LastModifiedBy = ts.LastModifiedBy
                })
                .FirstOrDefaultAsync();

            if (trainingStep == null)
            {
                return NotFound();
            }
            return Ok(trainingStep);
        }

        // GET: api/TrainingSteps/ByDepartment/5
        [HttpGet("by-department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<TrainingStepsDto>>> GetTrainingStepsByDepartment(int departmentId)
        {
            var trainingSteps = await dbContext.TrainingSteps
                .Where(ts => ts.DepartmentID == departmentId)
                .Select(ts => new TrainingStepsDto
                {
                    StepID = ts.StepID,
                    Step = ts.Step,
                    Item = ts.Item,
                    Description = ts.Description,
                    TraineeExpectation = ts.TraineeExpectation,
                    TrainerExpectation = ts.TrainerExpectation,
                    TrainingDuration = ts.TrainingDuration,
                    FilePath = ts.FilePath,
                    IsCompleted = ts.IsCompleted,
                    IsSignedOff = ts.IsSignedOff,
                    LastModifiedBy = ts.LastModifiedBy
                })
                .ToListAsync();

            if (trainingSteps == null || !trainingSteps.Any())
            {
                return NotFound();
            }

            return Ok(trainingSteps);
        }

        //// PUT: api/TrainingSteps/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateTrainingStep(int id, TrainingStep trainingStep)
        //{
        //    if (id != trainingStep.StepID)
        //    {
        //        return BadRequest();
        //    }

        //    dbContext.Entry(trainingStep).State = EntityState.Modified;

        //    try
        //    {
        //        await dbContext.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TrainingStepExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingStep(int id, UpdateTrainingStepsDto dto)
        {
            var trainingStep = await dbContext.TrainingSteps.FindAsync(id);
            if (trainingStep == null)
            {
                return NotFound();
            }

            trainingStep.Step = dto.Step;
            trainingStep.Item = dto.Item;
            trainingStep.Description = dto.Description;
            trainingStep.TraineeExpectation = dto.TraineeExpectation;
            trainingStep.TrainerExpectation = dto.TrainerExpectation;
            trainingStep.TrainingDuration = dto.TrainingDuration;
            trainingStep.FilePath = dto.FilePath;
            trainingStep.IsCompleted = dto.IsCompleted;
            trainingStep.IsSignedOff = dto.IsSignedOff;
            trainingStep.LastModifiedBy = dto.LastModifiedBy;

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

        //// POST: api/TrainingSteps
        [HttpPost]
        public async Task<ActionResult<CreateTrainingStepsDto>> CreateTrainingStep(CreateTrainingStepsDto dto)
        {

            var departmentExists = await dbContext.Departments.AnyAsync(d => d.DepartmentID == dto.DepartmentID);

            if (!departmentExists)
            {
                return BadRequest("The specified DepartmentID does not exist.");
            }

            var trainingStep = new TrainingStep
            {
                DepartmentID = dto.DepartmentID,
                Step = dto.Step,
                Item = dto.Item,
                Description = dto.Description,
                TraineeExpectation = dto.TraineeExpectation,
                TrainerExpectation = dto.TrainerExpectation,
                TrainingDuration = dto.TrainingDuration,
                FilePath = dto.FilePath,
                IsCompleted = dto.IsCompleted,
                IsSignedOff = dto.IsSignedOff,
                LastModifiedBy = dto.LastModifiedBy
            };

            dbContext.TrainingSteps.Add(trainingStep);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllTrainingSteps), new { id = trainingStep.StepID }, trainingStep);
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

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
            //getting all training steps from the db and mapping them to the TrainingStepsDto
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

            //returning a list of all training steps in the db
            return Ok(trainingSteps);
        }

        // GET: api/TrainingSteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingStepsDto>> GetTrainingStepById(int id)
        {
            //getting the single training step from the db using its stepId and mapping them to the TrainingStepsDto
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
            //getting all training steps for a department by their departmentId and mapping them to the TrainingStepsDto
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingStep(int id, UpdateTrainingStepsDto dto)
        {
            //getting the training step from the db using its stepId
            var trainingStep = await dbContext.TrainingSteps.FindAsync(id);
            if (trainingStep == null)
            {
                return NotFound();
            }

            //updating the training step properties with the data passed in from the dto
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
            //checks if the department exists before creating a training step
            var departmentExists = await dbContext.Departments.AnyAsync(d => d.DepartmentID == dto.DepartmentID);

            if (!departmentExists)
            {
                return BadRequest("The specified DepartmentID does not exist.");
            }

            //creating a new training step object using the data from the dto
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

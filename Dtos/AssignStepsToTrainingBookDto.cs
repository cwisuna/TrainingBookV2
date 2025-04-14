namespace TrainingBookV2.Dtos
{
    public class AssignStepsToTrainingBookDto
    {
        public int BookId { get; set; }
        public List<int> TrainingStepIds { get; set; }
    }
}

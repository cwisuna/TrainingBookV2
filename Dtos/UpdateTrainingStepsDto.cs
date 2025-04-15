namespace TrainingBookV2.Dtos
{
    public class UpdateTrainingStepsDto
    {
        public int Step { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string TraineeExpectation { get; set; }
        public string TrainerExpectation { get; set; }
        public int TrainingDuration { get; set; }
        public string FilePath { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsSignedOff { get; set; } = false;
        public int? LastModifiedBy { get; set; }
    }
}

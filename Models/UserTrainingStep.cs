namespace TrainingBookV2.Models
{
    public class UserTrainingStep
    {
        public int UserTrainingStepID { get; set; }
        public int UserTrainingBookID { get; set; }
        public UserTrainingBook UserTrainingBook { get; set; }
        public int StepID { get; set; }
        public TrainingStep Step { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public ICollection<TrainingNote> Notes { get; set; }
    }
}

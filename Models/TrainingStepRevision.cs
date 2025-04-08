namespace TrainingBookV2.Models
{
    public class TrainingStepRevision
    {
        public int RevisionID { get; set; }
        public int StepID { get; set; }
        public TrainingStep Step { get; set; }
        public int ModifiedByUserID { get; set; }
        public ApplicationUser ModifiedByUser { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string OldDescription { get; set; }
        public string OldTrainerExpectation { get; set; }
        public string OldTraineeExpectation { get; set; }
    }
}

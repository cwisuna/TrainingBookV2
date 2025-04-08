namespace TrainingBookV2.Models
{
    public class TrainingStep
    {
        public int StepID { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        public int Step { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string TraineeExpectation { get; set; }
        public string TrainerExpectation { get; set; }
        public int TrainingDuration { get; set; }
        public string FilePath { get; set; }

        public int? LastModifiedBy { get; set; }
        public ApplicationUser ModifiedByUser { get; set; }

        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? CompletedByUserID { get; set; }
        public ApplicationUser CompletedByUser { get; set; }

        public bool IsSignedOff { get; set; }
        public int? SignedOffByUserID { get; set; }
        public ApplicationUser SignedOffByUser { get; set; }
        public DateTime? SignedOffAt { get; set; }

        public ICollection<TrainingStepRevision> Revisions { get; set; }
        public ICollection<TrainingStepSignOff> SignOffs { get; set; }
        public ICollection<UserTrainingStep> UserTrainingSteps { get; set; }
    }
}

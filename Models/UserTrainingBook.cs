namespace TrainingBookV2.Models
{
    public class UserTrainingBook
    {
        public int UserTrainingBookID { get; set; }
        public int UserID { get; set; }
        public ApplicationUser User { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public ICollection<UserTrainingStep> TrainingSteps { get; set; }
    }
}

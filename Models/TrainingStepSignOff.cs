namespace TrainingBookV2.Models
{
    public class TrainingStepSignOff
    {
        public int SignOffID { get; set; }
        public int StepID { get; set; }
        public TrainingStep Step { get; set; }
        public int ManagerID { get; set; }
        public ApplicationUser Manager { get; set; }
        public DateTime SignedOffAt { get; set; } = DateTime.UtcNow;
        public string Comments { get; set; }
    }
}

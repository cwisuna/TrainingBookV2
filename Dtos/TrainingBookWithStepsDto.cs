namespace TrainingBookV2.Dtos
{
    public class TrainingBookWithStepsDto
    {
        public int BookID { get; set; }
        public int UserID { get; set; }
        public int DepartmentID { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TrainingStepsDto> TrainingSteps { get; set; } = new List<TrainingStepsDto>();
    }
}

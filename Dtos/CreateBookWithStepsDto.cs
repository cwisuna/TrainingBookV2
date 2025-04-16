namespace TrainingBookV2.Dtos
{
    public class CreateBookWithStepsDto
    {
        public int UserID { get; set; }
        public int DepartmentID { get; set; }
        public List<int> StepIDs { get; set; }
    }
}

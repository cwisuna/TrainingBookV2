namespace TrainingBookV2.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }
        public ICollection<TrainingStep> TrainingSteps { get; set; }
        public ICollection<UserTrainingBook> TrainingBooks { get; set; }
    }
}

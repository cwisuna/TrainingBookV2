namespace TrainingBookV2.Models
{
    public class TeamMember
    {
        public int TeamMemberID { get; set; }
        public int UserID { get; set; }
        public ApplicationUser User { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
    }
}

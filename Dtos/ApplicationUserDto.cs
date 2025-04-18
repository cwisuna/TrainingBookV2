namespace TrainingBookV2.Dtos
{
    public class ApplicationUserDto
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int? RoleID { get; set; }
        public string RoleName { get; set; }
        public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}

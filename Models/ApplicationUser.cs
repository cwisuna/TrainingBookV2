using System.Data;
using Microsoft.AspNetCore.Identity;

namespace TrainingBookV2.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? RoleID { get; set; }
        public Role Role { get; set; }

        public int? DepartmentID { get; set; }
        public Department Department { get; set; }

        public ICollection<TeamMember> TeamMemberships { get; set; }
        public ICollection<UserTrainingBook> TrainingBooks { get; set; }
    }
}

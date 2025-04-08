using Microsoft.AspNetCore.Identity;

namespace TrainingBookV2.Models
{
    public class Role : IdentityRole<int>
    {
        public ICollection<IdentityUserRole<int>> UserRoles { get; set; }
    }
}

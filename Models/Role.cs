using Microsoft.AspNetCore.Identity;

namespace TrainingBookV2.Models
{
    public class Role : IdentityUser<int>
    {
        public ICollection<ApplicationUser> Users { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace MariageApp.API.Models
{
    public class Role : IdentityRole<int>
    {
          public ICollection<UserRole> UserRoles { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Api.Shared.Entities
{
    public class User : IdentityUser
    {
        public String FirstName { get; set; } = null!;
        public String LastName { get; set; } = null!;
        public DateTime RegisteredAtUtc { get; set; }
        public DateTime? LastLoginAtUtc { get; set; }
        public bool IsBlocked { get; set; } = false;
    }
}

namespace UserManagement.Api.Shared.DTO
{
    public class UserDto
    {
        public String FirstName { get; set; } = null!;
        public String LastName { get; set; } = null!;
        public string Email { get; set; }
        public DateTime RegisteredAtUtc { get; set; }
        public DateTime? LastLoginAtUtc { get; set; }
        public bool IsBlocked { get; set; } = false;
    }
}

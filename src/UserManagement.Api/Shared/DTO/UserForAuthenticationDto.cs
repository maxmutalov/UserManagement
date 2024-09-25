using System.ComponentModel.DataAnnotations;

namespace UserManagement.Api.Shared.DTO
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Email is required")]
        public String? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public String? Password { get; set; }
    }
}

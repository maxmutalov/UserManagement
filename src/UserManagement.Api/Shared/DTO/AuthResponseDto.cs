namespace UserManagement.Api.Shared.DTO
{
    public class AuthResponseDto
    {
        public Boolean IsAuthSuccessful { get; set; }
        public String? Token { get; set; }
    }
}

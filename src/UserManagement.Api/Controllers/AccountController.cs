using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Shared.DTO;
using UserManagement.Api.Shared.Repositories;

namespace UserManagement.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserForRegistrationDto userDto)
        {
            var result = await _userRepository.RegisterUserAsync(userDto);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserForAuthenticationDto userDto)
        {
            var result = await _userRepository.LoginUserAsync(userDto);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost("blockuser/{id}")]
        public async Task<IActionResult> BlockUser(String id)
        {
            var result = await _userRepository.BlockUserAsync(id);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(String id)
        {
            var userResult = await _userRepository.GetUserByIdAsync(id);

            if (userResult.IsFailure)
                return BadRequest(userResult.Error);

            return Ok(userResult.Value);
        }

        [HttpGet("getallusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userResult = await _userRepository.GetAllUsersAsync();

            if (userResult.IsFailure)
                return BadRequest(userResult.Error);

            return Ok(userResult.Value);
        }
    }
}

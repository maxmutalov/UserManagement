using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Shared.DTO;
using UserManagement.Api.Shared.Repositories;

namespace UserManagement.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserRepository userRepository,
            ILogger<AccountController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserForRegistrationDto userDto)
        {
            _logger.LogInformation("AccountController.RegisterUser is starting.");

            var result = await _userRepository.RegisterUserAsync(userDto);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            _logger.LogInformation("AccountController.RegisterUser ended.");
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserForAuthenticationDto userDto)
        {
            _logger.LogInformation("AccountController.LoginUser is starting.");
            var result = await _userRepository.LoginUserAsync(userDto);

            if (result.IsFailure)
                return BadRequest(result.Error);

            _logger.LogInformation("AccountController.LoginUser ended.");
            return Ok(result.Value);
        }

        [HttpPost("blockuser/{id}")]
        public async Task<IActionResult> BlockUser(String id)
        {
            _logger.LogInformation("AccountController.BlockUser is starting.");
            var result = await _userRepository.BlockUserAsync(id);

            if (result.IsFailure)
                return BadRequest(result.Error);

            _logger.LogInformation("AccountController.BlockUser ended.");
            return Ok();
        }

        [HttpPost("unblockuser/{id}")]
        public async Task<IActionResult> UnblockUser(String id)
        {
            _logger.LogInformation("AccountController.UnblockUser is starting.");
            var result = await _userRepository.UnblockUserAsync(id);

            if (result.IsFailure)
                return BadRequest(result.Error);

            _logger.LogInformation("AccountController.UnblockUser ended.");
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(String id)
        {
            _logger.LogInformation("AccountController.GetUserById is starting.");
            var userResult = await _userRepository.GetUserByIdAsync(id);

            if (userResult.IsFailure)
                return BadRequest(userResult.Error);

            _logger.LogInformation("AccountController.GetUserById ended.");
            return Ok(userResult.Value);
        }

        [HttpGet("getallusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("AccountController.GetAllUsers is starting.");
            var userResult = await _userRepository.GetAllUsersAsync();

            if (userResult.IsFailure)
                return BadRequest(userResult.Error);

            _logger.LogInformation("AccountController.GetAllUsers ended.");
            return Ok(userResult.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(String id)
        {
            _logger.LogInformation("AccountController.DeleteUser is starting.");
            var result = await _userRepository.DeleteUserAsync(id);

            if (result.IsFailure)
                return BadRequest(result.Error);

            _logger.LogInformation("AccountController.DeleteUser ended.");
            return Ok();
        }
    }
}

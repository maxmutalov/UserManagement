using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Shared.Authentication;
using UserManagement.Api.Shared.DTO;
using UserManagement.Api.Shared.Entities;
using UserManagement.Api.Shared.Results;

namespace UserManagement.Api.Shared.Repositories
{
    public class UserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            UserManager<User> userManager,
            JwtHandler jwtHandler,
            ILogger<UserRepository> logger)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }

        public async Task<Result<String>> RegisterUserAsync(
            UserForRegistrationDto userDto)
        {
            _logger.LogInformation("UserRepository.RegisterUserAsync is starting.");
            var user = new User
            {
                UserName = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                RegisteredAtUtc = DateTime.UtcNow,
                Email = userDto.Email
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                return Result.Failure<String>(
                    new CustomError(
                        result.Errors.Select(e => e.Code).ToList(),
                        result.Errors.Select(e => e.Description).ToList()));
            }

            var userFromDb = await _userManager.FindByEmailAsync(userDto.Email);

            _logger.LogInformation("UserRepository.RegisterUserAsync ended.");
            return userFromDb!.Id;
        }

        public async Task<Result<AuthResponseDto>> LoginUserAsync(
            UserForAuthenticationDto userForAuthDto)
        {
            _logger.LogInformation("UserRepository.LoginUserAsync is starting.");
            var user = await _userManager.FindByNameAsync(userForAuthDto.Email!);

            if (user == null)
                return Result.Failure<AuthResponseDto>(CustomError.NullValue);
            else if (!await _userManager.CheckPasswordAsync(user, userForAuthDto.Password!))
                return Result.Failure<AuthResponseDto>(CustomError.InvalidParameters);

            user.LastLoginAtUtc = DateTime.UtcNow;
            user.IsBlocked = false;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return Result.Failure<AuthResponseDto>(
                    new CustomError(
                        updateResult.Errors.Select(e => e.Code).ToList(),
                        updateResult.Errors.Select(e => e.Description).ToList()));
            }

            var token = await _jwtHandler.CreateTokenAsync(user);

            var authResponse = new AuthResponseDto
            {
                IsAuthSuccessful = true,
                Token = token
            };

            _logger.LogInformation("UserRepository.LoginUserAsync ended.");
            return Result.Success(authResponse);
        }

        public async Task<Result> BlockUserAsync(String userId)
        {
            _logger.LogInformation("UserRepository.BlockUserAsync is starting.");
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(CustomError.UserNotFound);

            var updateSecurityStampResult = await _userManager.UpdateSecurityStampAsync(user);

            if (!updateSecurityStampResult.Succeeded)
            {
                return Result.Failure<String>(
                    new CustomError(
                        updateSecurityStampResult.Errors.Select(e => e.Code).ToList(),
                        updateSecurityStampResult.Errors.Select(e => e.Description).ToList()));
            }

            if (user.IsBlocked)
                return Result.Failure(CustomError.UserNotFound);

            user.IsBlocked = true;

            var updateUserResult = await _userManager.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
            {
                return Result.Failure<String>(
                    new CustomError(
                        updateUserResult.Errors.Select(e => e.Code).ToList(),
                        updateUserResult.Errors.Select(e => e.Description).ToList()));
            }

            _logger.LogInformation("UserRepository.BlockUserAsync ended.");
            return Result.Success();
        }

        public async Task<Result> UnblockUserAsync(String userId)
        {
            _logger.LogInformation("UserRepository.UnblockUserAsync is starting.");
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(CustomError.UserNotFound);

            if (!user.IsBlocked)
                return Result.Failure(CustomError.UsersIsNotBlocked);

            user.IsBlocked = false;

            var updateUserResult = await _userManager.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
            {
                return Result.Failure<String>(
                    new CustomError(
                        updateUserResult.Errors.Select(e => e.Code).ToList(),
                        updateUserResult.Errors.Select(e => e.Description).ToList()));
            }

            _logger.LogInformation("UserRepository.UnblockUserAsync ended.");
            return Result.Success();
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(String userId)
        {
            _logger.LogInformation("UserRepository.GetUserByIdAsync is starting.");
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure<UserDto>(CustomError.UserNotFound);

            var userDto = user.Adapt<UserDto>();

            _logger.LogInformation("UserRepository.GetUserByIdAsync ended.");
            return userDto;
        }

        public async Task<Result<List<UserDto>>> GetAllUsersAsync()
        {
            _logger.LogInformation("UserRepository.GetAllUsersAsync is starting.");
            var users = _userManager.Users;

            List<User> usersList = await users.ToListAsync();

            if (users == null)
                return Result.Failure<List<UserDto>>(CustomError.UsersNotFound);

            List<UserDto> userDtos = usersList.Select(x => x.Adapt<UserDto>()).ToList();

            _logger.LogInformation("UserRepository.GetAllUsersAsync ended.");
            return userDtos;
        }

        public async Task<Result> DeleteUserAsync(String userId)
        {
            _logger.LogInformation("UserRepository.DeleteUserAsync is starting.");
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure(CustomError.UserNotFound);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return Result.Failure<String>(
                    new CustomError(
                        result.Errors.Select(e => e.Code).ToList(),
                        result.Errors.Select(e => e.Description).ToList()));
            }

            _logger.LogInformation("UserRepository.DeleteUserAsync ended.");
            return Result.Success();
        }
    }
}

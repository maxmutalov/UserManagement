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

        public UserRepository(
            UserManager<User> userManager,
            JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        public async Task<Result<String>> RegisterUserAsync(
            UserForRegistrationDto userDto)
        {
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

            return userFromDb!.Id;
        }

        public async Task<Result<AuthResponseDto>> LoginUserAsync(
            UserForAuthenticationDto userForAuthDto)
        {
            var user = await _userManager.FindByNameAsync(userForAuthDto.Email!);

            if (user == null)
                return Result.Failure<AuthResponseDto>(CustomError.NullValue);
            else if (!await _userManager.CheckPasswordAsync(user, userForAuthDto.Password!))
                return Result.Failure<AuthResponseDto>(CustomError.InvalidParameters);

            user.LastLoginAtUtc = DateTime.UtcNow;

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

            return Result.Success(authResponse);
        }

        public async Task<Result> BlockUserAsync(String userId)
        {
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

            user.IsBlocked = true;

            var updateUserResult = await _userManager.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
            {
                return Result.Failure<String>(
                    new CustomError(
                        updateUserResult.Errors.Select(e => e.Code).ToList(),
                        updateUserResult.Errors.Select(e => e.Description).ToList()));
            }

            return Result.Success();
        }

        public async Task<Result<User>> GetUserByIdAsync(String userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure<User>(CustomError.UserNotFound);

            return user;
        }

        public async Task<Result<List<User>>> GetAllUsersAsync()
        {
            var users = _userManager.Users;

            List<User> usersList = await users.ToListAsync();

            if (users == null)
                return Result.Failure<List<User>>(CustomError.UsersNotFound);

            return usersList;
        }
    }
}

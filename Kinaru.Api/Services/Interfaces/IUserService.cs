using Kinaru.Shared.DTOs.Users;

namespace Kinaru.Api.Services.Interfaces;

public interface IUserService
{
    Task<UserProfileDto?> GetUserProfileAsync(Guid userId);
    Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateUserDto dto);
    Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
}

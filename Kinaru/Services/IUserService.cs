using Kinaru.Shared.DTOs.Users;
using Refit;

namespace Kinaru.Services;

public interface IUserService
{
    [Get("/api/users/me")]
    Task<UserProfileDto> GetMyProfileAsync();

    [Put("/api/users/me")]
    Task<UserProfileDto> UpdateMyProfileAsync([Body] UpdateUserDto dto);

    [Post("/api/users/me/change-password")]
    Task ChangePasswordAsync([Body] ChangePasswordDto dto);

    [Get("/api/users/{id}")]
    Task<UserProfileDto> GetUserProfileAsync(Guid id);
}

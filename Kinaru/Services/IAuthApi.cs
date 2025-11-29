using Kinaru.Shared.DTOs.Auth;
using Refit;

namespace Kinaru.Services;

public interface IAuthApi
{
    [Post("/api/auth/register")]
    Task<AuthResponseDto> RegisterAsync([Body] RegisterRequestDto request);

    [Post("/api/auth/login")]
    Task<AuthResponseDto> LoginAsync([Body] LoginRequestDto request);
}

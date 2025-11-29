using Kinaru.Shared.DTOs.Auth;

namespace Kinaru.Services;

public class AuthService
{
    private readonly IAuthApi _authApi;

    public AuthService(IAuthApi authApi)
    {
        _authApi = authApi;
    }

    public async Task<AuthResponseDto> LoginAsync(string email, string password)
    {
        var request = new LoginRequestDto
        {
            Email = email,
            Password = password
        };

        return await _authApi.LoginAsync(request);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        return await _authApi.RegisterAsync(request);
    }
}

using Kinaru.Api.Services.Interfaces;
using Kinaru.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Kinaru.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", async (RegisterRequestDto request, IAuthService authService) =>
        {
            try
            {
                var response = await authService.RegisterAsync(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .WithName("Register")
        .WithOpenApi();

        group.MapPost("/login", async (LoginRequestDto request, IAuthService authService) =>
        {
            try
            {
                var response = await authService.LoginAsync(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .WithName("Login")
        .WithOpenApi();
    }
}

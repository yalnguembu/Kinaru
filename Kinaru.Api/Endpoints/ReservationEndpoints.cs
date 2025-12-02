using System.Security.Claims;
using Kinaru.Api.Services.Interfaces;
using Kinaru.Shared.DTOs.Reservations;

namespace Kinaru.Api.Endpoints;

public static class ReservationEndpoints
{
    public static void MapReservationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reservations").WithTags("Reservations");

        group.MapPost("/", async (
            CreateReservationDto dto,
            IReservationService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var reservation = await service.CreateReservationAsync(userId, dto);
                return Results.Created($"/api/reservations/{reservation.Id}", reservation);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("CreateReservation")
        .WithOpenApi();

        group.MapGet("/me", async (
            IReservationService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var reservations = await service.GetUserReservationsAsync(userId);
                return Results.Ok(reservations);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("GetPropertyReservations")
        .WithOpenApi();

        group.MapPut("/{id:guid}/status", async (
            Guid id,
            UpdateReservationStatusDto dto,
            IReservationService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var reservation = await service.UpdateReservationStatusAsync(id, dto, userId);
                return Results.Ok(reservation);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("UpdateReservationStatus")
        .WithOpenApi();
    }

    private static Guid GetUserIdFromClaims(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in claims");
        
        return Guid.Parse(userIdClaim);
    }
}

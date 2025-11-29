using System.Security.Claims;
using Kinaru.Api.Services.Interfaces;
using Kinaru.Shared.DTOs.Agents;

namespace Kinaru.Api.Endpoints;

public static class AgentEndpoints
{
    public static void MapAgentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/agents").WithTags("Agents");

        group.MapGet("/", async (
            IAgentService service) =>
        {
            var agents = await service.GetAgentsAsync();
            return Results.Ok(agents);
        })
        .WithName("GetAgents")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (
            Guid id,
            IAgentService service) =>
        {
            var agent = await service.GetAgentByIdAsync(id);
            if (agent == null) return Results.NotFound();
            return Results.Ok(agent);
        })
        .WithName("GetAgentById")
        .WithOpenApi();

        group.MapGet("/profile", async (
            IAgentService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var agent = await service.GetAgentProfileAsync(userId);
                if (agent == null) return Results.NotFound();
                return Results.Ok(agent);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("GetMyAgentProfile")
        .WithOpenApi();

        group.MapPut("/profile", async (
            UpdateAgentDto dto,
            IAgentService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var agent = await service.CreateOrUpdateAgentProfileAsync(userId, dto);
                return Results.Ok(agent);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("UpdateMyAgentProfile")
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

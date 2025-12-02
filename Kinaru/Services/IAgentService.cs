using Kinaru.Shared.DTOs.Agents;
using Refit;

namespace Kinaru.Services;

public interface IAgentService
{
    [Get("/api/agents/profile")]
    Task<AgentDto> GetMyAgentProfileAsync();

    [Put("/api/agents/profile")]
    Task<AgentDto> UpdateMyAgentProfileAsync([Body] UpdateAgentDto dto);

    [Get("/api/agents/{id}")]
    Task<AgentDto> GetAgentByIdAsync(Guid id);

    [Get("/api/agents")]
    Task<List<AgentDto>> GetAgentsAsync();

    [Get("/api/agents/availability")]
    Task<List<AgentAvailabilityDto>> GetMyAvailabilityAsync([Query] DateTime? from = null, [Query] DateTime? to = null);

    [Post("/api/agents/availability")]
    Task<AgentAvailabilityDto> CreateAvailabilityAsync([Body] CreateAvailabilityDto dto);

    [Delete("/api/agents/availability/{id}")]
    Task DeleteAvailabilityAsync(Guid id);
}

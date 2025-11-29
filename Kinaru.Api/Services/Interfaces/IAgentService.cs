using Kinaru.Shared.DTOs.Agents;

namespace Kinaru.Api.Services.Interfaces;

public interface IAgentService
{
    Task<AgentDto?> GetAgentProfileAsync(Guid userId);
    Task<AgentDto> CreateOrUpdateAgentProfileAsync(Guid userId, UpdateAgentDto dto);
    Task<List<AgentDto>> GetAgentsAsync();
    Task<AgentDto?> GetAgentByIdAsync(Guid agentId);
}

using Kinaru.Api.Services.Interfaces;
using Kinaru.Database;
using Kinaru.Shared.DTOs.Agents;
using Kinaru.Shared.DTOs.Users;
using Kinaru.Shared.Entities;
using Kinaru.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Kinaru.Api.Services.Implementations;

public class AgentService : IAgentService
{
    private readonly KinaruDbContext _context;

    public AgentService(KinaruDbContext context)
    {
        _context = context;
    }

    public async Task<AgentDto?> GetAgentProfileAsync(Guid userId)
    {
        var agent = await _context.Agents
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.UserId == userId);

        return agent == null ? null : MapToDto(agent);
    }

    public async Task<AgentDto> CreateOrUpdateAgentProfileAsync(Guid userId, UpdateAgentDto dto)
    {
        var agent = await _context.Agents
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.UserId == userId);

        if (agent == null)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            // Ensure user is an agent
            if (user.Type != UserType.Agent)
            {
                user.Type = UserType.Agent;
            }

            agent = new Agent
            {
                UserId = userId,
                NomComplet = $"{user.Prenom} {user.Nom}",
                Agence = "Indépendant", // Default
                Specialite = dto.Specialites ?? string.Empty,
                Bio = dto.Biographie,
                ZoneIntervention = dto.ZoneIntervention ?? string.Empty,
                AnneesExperience = dto.AnneesExperience ?? 0,
                SiteWeb = dto.SiteWeb,
                Facebook = dto.Facebook,
                LinkedIn = dto.LinkedIn,
                Instagram = dto.Instagram,
                CreatedAt = DateTime.UtcNow
            };
            _context.Agents.Add(agent);
        }
        else
        {
            if (dto.Biographie != null) agent.Bio = dto.Biographie;
            if (dto.Specialites != null) agent.Specialite = dto.Specialites;
            if (dto.ZoneIntervention != null) agent.ZoneIntervention = dto.ZoneIntervention;
            if (dto.AnneesExperience.HasValue) agent.AnneesExperience = dto.AnneesExperience.Value;
            if (dto.SiteWeb != null) agent.SiteWeb = dto.SiteWeb;
            if (dto.Facebook != null) agent.Facebook = dto.Facebook;
            if (dto.LinkedIn != null) agent.LinkedIn = dto.LinkedIn;
            if (dto.Instagram != null) agent.Instagram = dto.Instagram;
        }

        await _context.SaveChangesAsync();

        // Reload to get user details if new
        if (agent.User == null)
        {
            await _context.Entry(agent).Reference(a => a.User).LoadAsync();
        }

        return MapToDto(agent);
    }

    public async Task<List<AgentDto>> GetAgentsAsync()
    {
        return await _context.Agents
            .Include(a => a.User)
            .Select(a => MapToDto(a))
            .ToListAsync();
    }

    public async Task<AgentDto?> GetAgentByIdAsync(Guid agentId)
    {
        var agent = await _context.Agents
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == agentId);

        return agent == null ? null : MapToDto(agent);
    }

    private static AgentDto MapToDto(Agent a)
    {
        return new AgentDto
        {
            Id = a.Id,
            UserId = a.UserId,
            Biographie = a.Bio ?? string.Empty,
            Specialites = a.Specialite,
            ZoneIntervention = a.ZoneIntervention,
            AnneesExperience = a.AnneesExperience,
            SiteWeb = a.SiteWeb,
            Facebook = a.Facebook,
            LinkedIn = a.LinkedIn,
            Instagram = a.Instagram,
            NoteMoyenne = a.NoteMoyenne,
            NombreAvis = a.NombreAvis,
            Verifie = a.Verifie,
            User = new UserBasicDto
            {
                Id = a.User.Id,
                Nom = a.User.Nom,
                Prenom = a.User.Prenom,
                Email = a.User.Email,
                Telephone = a.User.Telephone,
                PhotoProfil = a.User.PhotoProfil,
                Type = a.User.Type
            }
        };
    }
}

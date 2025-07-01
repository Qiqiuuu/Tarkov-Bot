using Microsoft.EntityFrameworkCore;
using TarkovBot.Database.Data;
using TarkovBot.Database.DTOs;
using TarkovBot.Database.Modules;

namespace TarkovBot.Database.Services;

public class TeamKillsService:ITeamKillsService
{
    private readonly DatabaseContext _context;

    public TeamKillsService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<GetTeamKill>> GetAllTeamKills(ulong guildId)
    {
        return await _context.TeamKills
            .Where(t => t.GuildId == guildId)
            .GroupBy(tk => tk.KillerId)
            .Select(group => new GetTeamKill
            {
                KillerId = group.Key,
                Kills = group.Select(tk => new KillDTO
                {
                    KilledId = tk.KilledId,
                    Description = tk.Description,
                    Date = tk.Date
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task AddTeamKill(ulong guildId, AddTeamKill teamKill)
    {
        _context.Add(new TeamKill
        {
            GuildId = guildId,
            KillerId = teamKill.KillerId,
            KilledId = teamKill.KilledId,
            Description = teamKill.Description,
            Date = teamKill.Date
        });
        await _context.SaveChangesAsync();
    }
}

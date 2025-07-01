using TarkovBot.Database.DTOs;
using TarkovBot.Database.Services;

namespace TarkovBot.Database.Controllers;

public class TeamKillsController
{
    private readonly ITeamKillsService _teamKillsService;

    public TeamKillsController(ITeamKillsService teamKillsService)
    {
        _teamKillsService = teamKillsService;
    }

    public async Task<List<GetTeamKill>> GetTeamKillsAsync(ulong guildId)
    {
        return  await _teamKillsService.GetAllTeamKills(guildId);
    }

    public async Task AddTeamKillAsync(ulong guildId, AddTeamKill addTeamKill)
    {
        await _teamKillsService.AddTeamKill(guildId, addTeamKill);
    }
}
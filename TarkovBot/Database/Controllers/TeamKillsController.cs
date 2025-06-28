using Microsoft.AspNetCore.Mvc;
using NetCord.Gateway;
using TarkovBot.Database.DTOs;
using TarkovBot.Database.Services;

namespace TarkovBot.Database.Controllers;

public class TeamKillsController
{
    private readonly ITeamKills _teamKills;

    public TeamKillsController(ITeamKills teamKills)
    {
        _teamKills = teamKills;
    }

    public async Task<List<GetTeamKill>> GetTeamKillsAsync(ulong guildId)
    {
        return  await _teamKills.GetAllTeamKills(guildId);
    }

    public async Task AddTeamKillAsync(ulong guildId, AddTeamKill addTeamKill)
    {
        await _teamKills.AddTeamKill(guildId, addTeamKill);
    }
}
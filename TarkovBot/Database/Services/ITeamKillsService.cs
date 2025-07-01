using TarkovBot.Database.DTOs;
using TarkovBot.Database.Modules;

namespace TarkovBot.Database.Services;

public interface ITeamKillsService
{
    public Task<List<GetTeamKill>> GetAllTeamKills(ulong guildId);
    public Task AddTeamKill(ulong guildId, AddTeamKill teamKill);
}
using TarkovBot.Database.Modules;

namespace TarkovBot.Database.DTOs;

public class GetTeamKill
{
    public ulong KillerId { get; set; }
    public List<KillDTO> Kills { get; set; }
}

public class KillDTO
{
    public ulong KilledId { get; set; }
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
}
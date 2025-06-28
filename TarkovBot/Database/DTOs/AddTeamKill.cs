namespace TarkovBot.Database.DTOs;

public class AddTeamKill
{
    public ulong KillerId { get; set; }
    public ulong KilledId { get; set; }
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
}
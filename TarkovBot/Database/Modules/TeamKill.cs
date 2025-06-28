namespace TarkovBot.Database.Modules;

public class TeamKill
{
    public int Id { get; set; }
    public ulong GuildId { get; set; }
    public ulong KillerId { get; set; }
    public ulong KilledId { get; set; }
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
}
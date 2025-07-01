namespace TarkovBot.Database.Modules;

public class TwitterPost
{
    public int Id { get; set; }
    public ulong ChannelId { get; set; }
    public int AccountId { get; set; }
    public int? LastTweetId { get; set; }
}
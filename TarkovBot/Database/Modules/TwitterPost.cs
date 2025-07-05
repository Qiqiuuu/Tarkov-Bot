namespace TarkovBot.Database.Modules;

public class TwitterPost
{
    public int Id { get; set; }
    public ulong ChannelId { get; set; }
    public long AccountId { get; set; }
    public long? LastTweetId { get; set; }
}
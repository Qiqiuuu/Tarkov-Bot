namespace TarkovBot.Database.DTOs;

public class GetTweet
{
    public long AccountId { get; set; }
    public long? LastTweetId { get; set; }
    public List<ulong> ChannelIds { get; set; }
}
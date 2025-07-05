using System.Collections.Concurrent;
using TarkovBot.Database.DTOs;

namespace TarkovBot.Database.Services;

public interface ITwitterRetrieverService
{
    public Task AddTracking(ulong channelId,long accountId);
    public Task<List<GetTweet>> GetRecentTweets();
    
    public Task UpdateTweet(long AccountId, long TweetId);
}
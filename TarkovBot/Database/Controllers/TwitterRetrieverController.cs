using System.Collections.Concurrent;
using TarkovBot.Database.DTOs;
using TarkovBot.Database.Services;

namespace TarkovBot.Database.Controllers;

public class TwitterRetrieverController
{
    private readonly ITwitterRetrieverService _twitterRetrieverService;

    public TwitterRetrieverController(ITwitterRetrieverService twitterRetrieverService)
    {
        _twitterRetrieverService = twitterRetrieverService;
    }

    public async Task AddTracking(ulong channelId, long accountId)
    {
        await _twitterRetrieverService.AddTracking(channelId, accountId);
    }

    public async Task<List<GetTweet>> GetRecentTweets()
    {
        return await _twitterRetrieverService.GetRecentTweets();
    }

    public async Task UpdateTweets(long accountId, long tweetId)
    {
        await _twitterRetrieverService.UpdateTweet(accountId, tweetId);
    }
} 
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using TarkovBot.Database.Data;
using TarkovBot.Database.DTOs;
using TarkovBot.Database.Modules;

namespace TarkovBot.Database.Services;

public class TwitterRetrieverService:ITwitterRetrieverService
{
    private readonly DatabaseContext _context;

    public TwitterRetrieverService(DatabaseContext context)
    {
        _context = context;
    }

    public Task AddTracking(ulong channelId,long accountId)
    {
        var tracking = new TwitterPost()
        {
            ChannelId = channelId,
            AccountId = accountId,
            LastTweetId = null
        };
        _context.TwitterPosts.Add(tracking);
        return _context.SaveChangesAsync();
    }

    public async Task<List<GetTweet>> GetRecentTweets()
    {
        return await _context.TwitterPosts
            .GroupBy(e => e.AccountId)
            .Select(x => new GetTweet()
            {
                AccountId = x.Key,
                LastTweetId = x.First().LastTweetId,
                ChannelIds = x.Select(y => y.ChannelId).Distinct().ToList()
            }).ToListAsync();
    }

    public async Task UpdateTweet(long AccountId, long TweetId)
    {
        await _context.TwitterPosts
            .Where(e=>e.AccountId==AccountId)
            .ExecuteUpdateAsync(e=>e
                .SetProperty(p => p.LastTweetId,TweetId));
    }
}
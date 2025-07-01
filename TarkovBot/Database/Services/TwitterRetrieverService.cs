using TarkovBot.Database.Data;
using TarkovBot.Database.Modules;

namespace TarkovBot.Database.Services;

public class TwitterRetrieverService:ITwitterRetrieverService
{
    private readonly DatabaseContext _context;

    public TwitterRetrieverService(DatabaseContext context)
    {
        _context = context;
    }

    public Task AddTracking(ulong channelId,int accountId)
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
}
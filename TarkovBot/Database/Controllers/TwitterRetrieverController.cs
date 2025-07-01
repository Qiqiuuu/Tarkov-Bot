using TarkovBot.Database.Services;

namespace TarkovBot.Database.Controllers;

public class TwitterRetrieverController
{
    private readonly ITwitterRetrieverService _twitterRetrieverService;

    public TwitterRetrieverController(ITwitterRetrieverService twitterRetrieverService)
    {
        _twitterRetrieverService = twitterRetrieverService;
    }

    public async Task AddTracking(ulong channelId, int accountId)
    {
        await _twitterRetrieverService.AddTracking(channelId, accountId);
    }
    
} 
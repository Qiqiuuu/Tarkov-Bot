using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Rest;
using TarkovBot.Database.Controllers;
using Tweetinvi;
using Tweetinvi.Parameters;
using TarkovBot.Database.DTOs;
using Tweetinvi.Models;
using Tweetinvi.Parameters.V2;

namespace TarkovBot.Services;



public class TwitterMonitoringService:IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILogger<TwitterMonitoringService> _logger;
    private readonly TimeSpan _pollingInterval;
    private List<GetTweet> _recentTweets;
    private readonly TwitterHTTPClient _twitterClient;
    private readonly RestClient _discordClient;
    private readonly TwitterRetrieverController _twitterRetrieverController;


    public TwitterMonitoringService(
        ILogger<TwitterMonitoringService> logger, 
        TwitterRetrieverController twitterRetrieverController,
        RestClient discordClient,
        IConfiguration configuration,
        TwitterHTTPClient twitterClient)
    {
        _logger = logger;
        _twitterRetrieverController = twitterRetrieverController;
        _discordClient = discordClient;
        _twitterClient = twitterClient;
        _pollingInterval = TimeSpan.FromHours(configuration.GetValue<int>("TwitterMonitoringService:PollingInterval"));
        
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TwitterMonitoringService is starting.");
        _recentTweets = await _twitterRetrieverController.GetRecentTweets();
        _timer = new Timer(async (e) => await CheckTweets(e),null,TimeSpan.Zero,_pollingInterval);
    }
    private async Task CheckTweets(object? state)
    {
        _logger.LogInformation("Checking Tweets...");
        foreach (var recentTweet in _recentTweets)
        {
            var user = recentTweet.AccountId;
            var lastTweet = recentTweet.LastTweetId;

            try
            {
                var latestTweets = await _twitterClient.GetTweetsAsync(user, lastTweet ?? null);

                if (latestTweets != null && latestTweets.Any())
                {
                    var newestTweetId = latestTweets.Max(t => long.Parse(t.Id));
                    await _twitterRetrieverController.UpdateTweets(newestTweetId, recentTweet.AccountId);

                    foreach (var tweet in latestTweets.OrderBy(t => long.Parse(t.Id)))
                    {
                        string tweetUrl = $"https://twitter.com/{user}/status/{tweet.Id}";
                        _logger.LogInformation($"New tweet from {user}: {tweet.Text}");

                        foreach (var channelId in recentTweet.ChannelIds)
                        {
                            var channel = await _discordClient.GetChannelAsync(channelId);
                            if (channel is TextChannel textChannel)
                            {
                                await textChannel.SendMessageAsync(tweetUrl);
                            }
                            else
                            {
                                _logger.LogInformation($"Channel {channelId} not found");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Checking Tweets failed: {ex.Message}");
            }
        }
        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TwitterMonitoringService is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async Task<string> GetIdOfUser(string username)
    {
        try
        {
            var userId = await _twitterClient.GetUserByName(username);
            if (userId == null) throw new EntityNotFoundException();
            return userId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user ID");
        }
        return String.Empty;
    }
}
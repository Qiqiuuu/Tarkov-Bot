
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using TarkovBot.Database.Controllers;
using TarkovBot.Services;
using Tweetinvi;

namespace TarkovBot.SlashCommands;

public class Twitter:ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly TwitterRetrieverController _twitterRetriever;
    private readonly TwitterMonitoringService _tweetMonitoringService;

    public Twitter(TwitterRetrieverController twitterRetriever, TwitterMonitoringService tweetMonitoringService, TwitterAccount twitterAccount)
    {
        _twitterRetriever = twitterRetriever;
        _tweetMonitoringService = tweetMonitoringService;
    }

    [SlashCommand("setchannel", "Set channel for automatic tweets")]
    public async Task SetChannel([SlashCommandParameter(Name = "account",Description = "Twitter account u want to track")] string twitteraccount)
    {
        await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
        try
        {
            var accountId = await _tweetMonitoringService.GetIdOfUser(twitteraccount);
            await _twitterRetriever.AddTracking(Context.Channel.Id, long.Parse(accountId));
            await Context.Interaction.ModifyResponseAsync(option =>
            {
                option.Content = $"Account {accountId} is now tracked on this channel.";
            });
        }
        catch (EntityNotFoundException e)
        {
            await Context.Interaction.ModifyResponseAsync(option =>
            {
                option.Content = $"Couldn't find user {twitteraccount}";
            });
        }
    }
}
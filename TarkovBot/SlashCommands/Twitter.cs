using Microsoft.Extensions.Logging;
using NetCord.Services.ApplicationCommands;
using TarkovBot.Database.Controllers;

namespace TarkovBot.SlashCommands;

public class Twitter:ApplicationCommandModule<SlashCommandContext>
{
    private readonly TwitterRetrieverController _twitterRetriever;

    public Twitter(TwitterRetrieverController twitterRetriever)
    {
        _twitterRetriever = twitterRetriever;
    }
    
    
}
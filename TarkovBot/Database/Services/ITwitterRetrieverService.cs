namespace TarkovBot.Database.Services;

public interface ITwitterRetrieverService
{
    public Task AddTracking(ulong channelId,int accountId);
}
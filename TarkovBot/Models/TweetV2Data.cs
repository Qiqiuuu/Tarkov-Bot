using Newtonsoft.Json;

namespace TarkovBot.Models;

public class TweetV2Data
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("text")]
    public string Text { get; set; } = string.Empty;

    [JsonProperty("edit_history_tweet_ids")]
    public List<string> EditHistoryTweetIds { get; set; } = new();
}
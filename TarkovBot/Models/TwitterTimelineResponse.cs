using Newtonsoft.Json;

namespace TarkovBot.Models;

public class TwitterTimelineResponse
{
    [JsonProperty("data")]
    public List<TweetV2Data>? Data { get; set; }

    [JsonProperty("meta")]
    public TwitterTimelineMeta? Meta { get; set; }

    [JsonProperty("errors")]
    public List<TwitterApiError>? Errors { get; set; }
    
    [JsonProperty("includes")]
    public TwitterIncludes? Includes { get; set; }
}
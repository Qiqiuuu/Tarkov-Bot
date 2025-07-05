using Newtonsoft.Json;

namespace TarkovBot.Models;

public class TwitterUserLookupResponse
{
    [JsonProperty("data")]
    public TwitterUser? Data { get; set; }
}
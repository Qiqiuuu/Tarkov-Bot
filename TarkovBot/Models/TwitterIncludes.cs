using Newtonsoft.Json;

namespace TarkovBot.Models;

public class TwitterIncludes
{
    [JsonProperty("users")]
    public List<TwitterUser>? Users { get; set; }
}
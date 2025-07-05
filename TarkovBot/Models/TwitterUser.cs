using Newtonsoft.Json;

namespace TarkovBot.Models;

public class TwitterUser
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;
}
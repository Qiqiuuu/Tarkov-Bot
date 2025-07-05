using Newtonsoft.Json;

namespace TarkovBot.Models;

public class TwitterApiError
{
    [JsonProperty("value")]
    public string? Value { get; set; }

    [JsonProperty("detail")]
    public string? Detail { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("parameter")]
    public string? Parameter { get; set; }

    [JsonProperty("resource_id")]
    public string? ResourceId { get; set; }

    [JsonProperty("resource_type")]
    public string? ResourceType { get; set; }

    [JsonProperty("section")]
    public string? Section { get; set; }
}

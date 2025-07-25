﻿using Newtonsoft.Json;

namespace TarkovBot.Models;

public class TwitterTimelineMeta
{
    [JsonProperty("next_token")]
    public string? NextToken { get; set; }

    [JsonProperty("result_count")]
    public int ResultCount { get; set; }

    [JsonProperty("newest_id")]
    public string? NewestId { get; set; }

    [JsonProperty("oldest_id")]
    public string? OldestId { get; set; }
}
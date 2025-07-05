using System.Web;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TarkovBot.Models;
using Tweetinvi.Client;
using Tweetinvi.Core.Models;
using Tweetinvi.WebLogic;

namespace TarkovBot.Services;

public class TwitterHTTPClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TwitterHTTPClient> _logger;

    public TwitterHTTPClient(HttpClient httpClient,ILogger<TwitterHTTPClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetUserByName(string username)
    {
        var url = $"2/users/by/username/{username}";
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress, url));
        
        try
        {
            var response = await _httpClient.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogDebug($"Received Twitter API response: {content}");
            
            var parsed = JsonConvert.DeserializeObject<TwitterUserLookupResponse>(content);
            return parsed?.Data?.Id ?? throw new Exception("User not found in response.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"HTTP request to Twitter API failed: {ex.Message}");
            if (ex.StatusCode.HasValue)
            {
                _logger.LogError($"Status Code: {ex.StatusCode.Value}");
            }
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching tweets from Twitter API.");
            throw;
        }
    }
    

    public async Task<List<TweetV2Data>?> GetTweetsAsync(long accountId,long? sinceId = null)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "max_results", "5" },
            { "expansions", "author_id" },
            { "user.fields", "username" }
        };
        if (sinceId.HasValue)
        {
            queryParams.Add("since_id", sinceId.Value.ToString());
        }
        var url = $"2/users/{accountId}/tweets";
        var uriBuilder = new UriBuilder(new Uri(_httpClient.BaseAddress, url));
        var query = HttpUtility.ParseQueryString(string.Empty);
        foreach (var param in queryParams)
        {
            query[param.Key] = param.Value;
        }
        uriBuilder.Query = query.ToString();

        _logger.LogInformation($"Sending GET request to {uriBuilder.Uri}");

        try
        {
            var response = await _httpClient.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogDebug($"Received Twitter API response: {content}");
            
            var parsed = JsonConvert.DeserializeObject<TwitterTimelineResponse>(content);
            return parsed?.Data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"HTTP request to Twitter API failed: {ex.Message}");
            if (ex.StatusCode.HasValue)
            {
                _logger.LogError($"Status Code: {ex.StatusCode.Value}");
            }
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching tweets from Twitter API.");
            throw;
        }
        
    }
}
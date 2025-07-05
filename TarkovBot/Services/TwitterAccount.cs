using Tweetinvi;
using Tweetinvi.Models;

namespace TarkovBot.Services;

public class TwitterAccount
{ 
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string BearerToken { get; set; }

        public TwitterAccount()
        {
        }

        public TwitterClient ReturnNewClient()
        {
                var appCred = new TwitterCredentials(ConsumerKey, ConsumerSecret)
                {
                        BearerToken = BearerToken
                };
                return new TwitterClient(appCred);
        }
}
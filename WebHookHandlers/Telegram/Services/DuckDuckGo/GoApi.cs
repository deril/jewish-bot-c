using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHanders.Telegram.Services.DuckDuckGo
{
    public class GoApi : ApiService
    {
        private const string BaseUrl = "http://api.duckduckgo.com";

        public override string BuildEndpointRoute(string term)
        {
            var parameters = new Dictionary<string, string>
            {
                {"q", term},
                {"format", "json"}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
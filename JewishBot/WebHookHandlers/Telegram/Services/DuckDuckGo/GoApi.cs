using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.DuckDuckGo
{
    public class GoApi : ApiServiceJson
    {
        const string BaseUrl = "http://api.duckduckgo.com";

        public override string BuildEndpointRoute(string[] arguments)
        {
            var parameters = new Dictionary<string, string>
            {
                {"q", String.Join("", arguments)},
                {"format", "json"}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
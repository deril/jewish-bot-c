using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.DuckDuckGo
{
    public class GoApi : ApiService
    {
		const string BaseUrl = "http://api.duckduckgo.com";

		public override string BuildEndpointRoute(string argument)
        {
            var parameters = new Dictionary<string, string>
            {
                {"q", argument},
                {"format", "json"}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
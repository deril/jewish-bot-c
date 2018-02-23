namespace JewishBot.WebHookHandlers.Telegram.Services.DuckDuckGo
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.WebUtilities;

    public class GoApi : ApiServiceJson
    {
        private const string BaseUrl = "http://api.duckduckgo.com";

        protected override string BuildEndpointRoute(string[] arguments)
        {
            var parameters = new Dictionary<string, string>
            {
                { "q", string.Join(string.Empty, arguments) },
                { "format", "json" }
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}

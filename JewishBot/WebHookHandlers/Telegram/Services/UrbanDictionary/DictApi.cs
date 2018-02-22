namespace JewishBot.WebHookHandlers.Telegram.Services.UrbanDictionary
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.WebUtilities;

    public class DictApi : ApiServiceJson
    {
        private const string BaseUrl = "http://api.urbandictionary.com/v0/define";

        public override string BuildEndpointRoute(string[] arguments)
        {
            var parameters = new Dictionary<string, string>
            {
                { "term", string.Join(string.Empty, arguments) }
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
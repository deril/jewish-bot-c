using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.UrbanDictionary
{
    public class DictApi : ApiServiceJson
    {
        const string BaseUrl = "http://api.urbandictionary.com/v0/define";

        public override string BuildEndpointRoute(string[] arguments)
        {
            var parameters = new Dictionary<string, string>
            {
                {"term", String.Join("", arguments)}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
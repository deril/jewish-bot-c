using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.UrbanDictionary
{
    public class DictApi : ApiService
    {
        private const string BaseUrl = "http://api.urbandictionary.com/v0/define";

        public override string BuildEndpointRoute(string term)
        {
            var parameters = new Dictionary<string, string>
            {
                {"term", term}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
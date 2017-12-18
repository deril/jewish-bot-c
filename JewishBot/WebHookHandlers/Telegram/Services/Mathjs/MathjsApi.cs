using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.Mathjs
{
    public class MathjsApi : ApiServiceJson
    {
        const string BaseUrl = "http://math.leftforliving.com/query";

        public override string BuildEndpointRoute(string[] arguments)
        {
            var parameters = new Dictionary<string, string>
            {
                {"question", String.Join("", arguments)}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
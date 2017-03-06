using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.Mathjs
{
    public class MathjsApi : ApiService
    {
        private const string BaseUrl = "http://math.leftforliving.com/query";

        public override string BuildEndpointRoute(string question)
        {
            var parameters = new Dictionary<string, string>
            {
                {"question", question}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
namespace JewishBot.WebHookHandlers.Telegram.Services.Mathjs
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.WebUtilities;

    public class MathjsApi : ApiServiceJson
    {
        private const string BaseUrl = "http://math.leftforliving.com/query";

        public override string BuildEndpointRoute(string[] arguments)
        {
            var parameters = new Dictionary<string, string>
            {
                { "question", string.Join(string.Empty, arguments) }
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
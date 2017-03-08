using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.Mathjs
{
    public class MathjsApi : ApiService
    {
		const string BaseUrl = "http://math.leftforliving.com/query";

		public override string BuildEndpointRoute(string argument)
        {
            var parameters = new Dictionary<string, string>
            {
                {"question", argument}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
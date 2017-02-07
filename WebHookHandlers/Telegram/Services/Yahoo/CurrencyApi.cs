using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHanders.Telegram.Services.Yahoo
{
    public class CurrencyApi : ApiService
    {
        private const string BaseUrl = "https://query.yahooapis.com/v1/public/yql";

        public override string BuildEndpointRoute(string pair)
        {
            var parameters = new Dictionary<string, string>
            {
                {"q", $"select Rate from yahoo.finance.xchange where pair in (\"{pair}\")"},
                {"format", "json"},
                {"env", "store://datatables.org/alltableswithkeys"}
            };


            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
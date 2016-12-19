using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.Services.Yahoo {
    public class CurrencyApi : ApiService {
        private string baseUrl = "https://query.yahooapis.com/v1/public/yql";

        public override string buildEndpointRoute(string pair) {
            var parameters = new Dictionary<string, string>();

            parameters.Add("q", $"select Rate from yahoo.finance.xchange where pair in (\"{pair}\")");
            parameters.Add("format", "json");
            parameters.Add("env", "store://datatables.org/alltableswithkeys");

            return QueryHelpers.AddQueryString(baseUrl, parameters);
        }
    }
}
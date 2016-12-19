using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.Services.DuckDuckGo {
    public class GoApi : ApiService {
        private string baseUrl = "http://api.duckduckgo.com";

        public override string buildEndpointRoute(string term) {
            var parameters = new Dictionary<string, string>();

            parameters.Add("q", term);
            parameters.Add("format", "json");

            return QueryHelpers.AddQueryString(baseUrl, parameters);
        }
    }
}
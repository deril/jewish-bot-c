using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.Services.UrbanDictionary {
    public class DictApi : ApiService {
        private string baseUrl = "http://api.urbandictionary.com/v0/define";

        public override string buildEndpointRoute(string term) {
            var parameters = new Dictionary<string, string>();

            parameters.Add("term", term);

            return QueryHelpers.AddQueryString(baseUrl, parameters);
        }
    }
}
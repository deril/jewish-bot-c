using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.Services.Poem {
    public class PoemApi : ApiService {
        private string baseUrl = "https://poem.alv.in/api/generate";

        public override string buildEndpointRoute(string term) {
            return baseUrl;
        }

        public async void Like(string hashid) {
            try {
                queryUrl = buildEndpointLikeRoute(hashid);
                response = await httpClient.GetAsync(queryUrl);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();

                this.queryUrl = this.baseUrl;
            } catch {
                // TODO: implement here logging
            }
        }

        private string buildEndpointLikeRoute(string hashid) {
            var parameters = new Dictionary<string, string>();

            parameters.Add("hashid", hashid);

            return QueryHelpers.AddQueryString(baseUrl, parameters);
        }
    }
}
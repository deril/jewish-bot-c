namespace JewishBot.WebHookHandlers.Telegram.Services.Poem
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.WebUtilities;

    public class PoemApi : ApiServiceJson
    {
        private const string BaseUrl = "https://poem.alv.in/api/generate";
        private readonly HttpClient httpClient = new HttpClient();

        public async Task Like(string hashid)
        {
            var queryUrl = BuildEndpointLikeRoute(hashid);
            var response = await this.httpClient.GetAsync(queryUrl);
            response.EnsureSuccessStatusCode();
            this.Content = await response.Content.ReadAsStringAsync();
        }

        protected override string BuildEndpointRoute(string[] arguments) => BaseUrl;

        private static string BuildEndpointLikeRoute(string hashid)
        {
            var parameters = new Dictionary<string, string>
            {
                { "hashid", hashid }
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
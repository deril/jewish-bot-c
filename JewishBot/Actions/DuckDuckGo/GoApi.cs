namespace JewishBot.Actions.DuckDuckGo
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.WebUtilities;
    using Newtonsoft.Json;

    public class GoApi
    {
        private readonly IHttpClientFactory clientFactory;

        public GoApi(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<QueryModel> InvokeAsync(IEnumerable<string> arguments)
        {
            var client = this.clientFactory.CreateClient("duckduckgo");
            var query = new Dictionary<string, string>
            {
                {"q", string.Join(string.Empty, arguments)},
                {"format", "json"}
            };

            try
            {
                var response =
                    await client.GetStringAsync(
                        new Uri(QueryHelpers.AddQueryString(client.BaseAddress.ToString(), query)));
                return JsonConvert.DeserializeObject<QueryModel>(response);
            }
            catch (HttpRequestException e)
            {
                return new QueryModel();
            }
        }
    }
}
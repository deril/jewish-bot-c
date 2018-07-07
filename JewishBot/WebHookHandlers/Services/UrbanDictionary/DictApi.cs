namespace JewishBot.WebHookHandlers.Services.UrbanDictionary
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.WebUtilities;
    using Newtonsoft.Json;

    public class DictApi
    {
        private readonly IHttpClientFactory clientFactory;

        public DictApi(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<QueryModel> InvokeAsync(IReadOnlyCollection<string> arguments)
        {
            var client = this.clientFactory.CreateClient("urbandictionary");
            var query = new Dictionary<string, string>
            {
                { "term", string.Join(string.Empty, arguments) }
            };
            var route = new UriBuilder(client.BaseAddress)
            {
                Path = "v0/define"
            };

            try
            {
                var response = await client.GetStringAsync(new Uri(QueryHelpers.AddQueryString(route.Uri.ToString(), query)));
                return JsonConvert.DeserializeObject<QueryModel>(response);
            }
            catch (HttpRequestException e)
            {
                return new QueryModel();
            }
        }
    }
}
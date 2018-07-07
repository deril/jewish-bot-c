namespace JewishBot.WebHookHandlers.Services.Poem
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class PoemApi
    {
        private readonly IHttpClientFactory clientFactory;

        public PoemApi(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<QueryModel> InvokeAsync()
        {
            var client = this.clientFactory.CreateClient("poemapi");
            var route = new UriBuilder(client.BaseAddress)
            {
                Path = "api/generate"
            };

            try
            {
                var response = await client.GetStringAsync(route.Uri);
                return JsonConvert.DeserializeObject<QueryModel>(response);
            }
            catch (HttpRequestException e)
            {
                return new QueryModel();
            }
        }
    }
}
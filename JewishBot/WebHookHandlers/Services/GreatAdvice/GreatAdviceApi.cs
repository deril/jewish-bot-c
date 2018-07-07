namespace JewishBot.WebHookHandlers.Services.GreatAdvice
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class GreatAdviceApi
    {
        private readonly IHttpClientFactory clientFactory;

        public GreatAdviceApi(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<QueryModel> InvokeAsync()
        {
            var client = this.clientFactory.CreateClient("greadadvice");
            var route = new UriBuilder(client.BaseAddress)
            {
                Path = "api/random"
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
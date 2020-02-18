namespace JewishBot.Actions.GoogleMaps
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Attributes;
    using Microsoft.AspNetCore.WebUtilities;
    using Newtonsoft.Json;

    public class GoogleMapsApi
    {
        private readonly string apiKey;
        private readonly IHttpClientFactory clientFactory;

        public GoogleMapsApi(IHttpClientFactory clientFactory, string apiKey)
        {
            this.clientFactory = clientFactory;
            this.apiKey = apiKey;
        }

        [RequestRateLimit(Name = "Limit Request Number", Seconds = 2)]
        public async Task<QueryModel> InvokeAsync(IReadOnlyCollection<string> arguments)
        {
            var client = this.clientFactory.CreateClient("googleapis");
            var query = new Dictionary<string, string>
            {
                { "address", string.Join(string.Empty, arguments) },
                { "key", this.apiKey }
            };
            var route = new UriBuilder(client.BaseAddress)
            {
                Path = "maps/api/geocode/json"
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
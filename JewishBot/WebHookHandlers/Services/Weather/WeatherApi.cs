namespace JewishBot.WebHookHandlers.Services.Weather
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.WebUtilities;

    public class WeatherApi
    {
        private readonly IHttpClientFactory clientFactory;

        public WeatherApi(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<string> InvokeAsync(IReadOnlyCollection<string> arguments)
        {
            var client = this.clientFactory.CreateClient("weatherapi");
            var query = new Dictionary<string, string>
            {
                { "format", "3" }
            };
            var route = new UriBuilder(client.BaseAddress)
            {
                Path = string.Join("+", arguments)
            };
            try
            {
                var response = await client.GetStringAsync(new Uri(QueryHelpers.AddQueryString(route.Uri.ToString(), query)));
                return response;
            }
            catch (HttpRequestException e)
            {
                return string.Empty;
            }
        }
    }
}

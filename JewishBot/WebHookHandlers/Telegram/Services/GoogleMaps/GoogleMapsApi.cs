namespace JewishBot.WebHookHandlers.Telegram.Services.GoogleMaps
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.WebUtilities;

    public class GoogleMapsApi : ApiServiceJson
    {
        private const string BaseUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        private readonly string apiKey;

        public GoogleMapsApi(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public override string BuildEndpointRoute(string[] arguments)
        {
            var parameters = new Dictionary<string, string>
            {
                { "address", string.Join(string.Empty, arguments) },
                { "key", this.apiKey }
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
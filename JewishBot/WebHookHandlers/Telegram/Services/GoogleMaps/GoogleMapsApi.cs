using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.GoogleMaps
{
    public class GoogleMapsApi : ApiServiceJson
    {
        const string BaseUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        string ApiKey { get; }

        public GoogleMapsApi(string apiKey)
        {
            ApiKey = apiKey;
        }
        public override string BuildEndpointRoute(string[] arguments)
        {
            
            var parameters = new Dictionary<string, string>
            {
                {"address", String.Join("", arguments)},
                {"key", ApiKey}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
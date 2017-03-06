using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.GoogleMaps
{
    public class GoogleMapsApi : ApiService
    {
        private const string BaseUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        private string ApiKey { get; }

        public GoogleMapsApi(string apiKey)
        {
            ApiKey = apiKey;
        }
        public override string BuildEndpointRoute(string term)
        {
            
            var parameters = new Dictionary<string, string>
            {
                {"address", term},
                {"key", ApiKey}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
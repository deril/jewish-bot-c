using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.Services.GoogleMaps
{
    public class GoogleMapsApi : ApiService
    {
        private const string BaseUrl = "https://maps.googleapis.com/maps/api/geocode/json";

        public override string BuildEndpointRoute(string term)
        {
            var parameters = new Dictionary<string, string>
            {
                {"address", term},
                {"key", Program.Configuration["googleApiKey"]}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
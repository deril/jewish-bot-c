namespace JewishBot.WebHookHandlers.Services.GoogleMaps
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class QueryModel
    {
        [JsonProperty("results")]
        public List<Result> Results { get; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
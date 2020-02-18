namespace JewishBot.Actions.GoogleMaps
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class QueryModel
    {
        [JsonProperty("results")]
        public IList<Result> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
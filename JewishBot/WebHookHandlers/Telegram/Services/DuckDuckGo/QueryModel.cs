namespace JewishBot.WebHookHandlers.Telegram.Services.DuckDuckGo
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class QueryModel
    {
        [JsonProperty("AbstractText")]
        public string AbstractText { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("AbstractURL")]
        public string AbstractUrl { get; set; }

        [JsonProperty("Redirect")]
        public string Redirect { get; set; }

        [JsonProperty("RelatedTopics")]
        public List<RelatedTopic> RelatedTopics { get; set; }
    }
}
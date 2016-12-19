using System.Collections.Generic;
using Newtonsoft.Json;

namespace JewishBot.Services.DuckDuckGo {
    public class QueryModel {
        [JsonProperty("AbstractText")]
        public string AbstractText { get; set; }
        [JsonProperty("Type")]
        public string Type { get; set; }
        [JsonProperty("AbstractURL")]
        public string AbstractURL { get; set; }
        [JsonProperty("Redirect")]
        public string Redirect { get; set; }
        [JsonProperty("RelatedTopics")]
        public List<RelatedTopic> RelatedTopics { get; set; }
    }

    public class RelatedTopic {
        [JsonProperty("Text")]
        public string Text { get; set; }
    }
}
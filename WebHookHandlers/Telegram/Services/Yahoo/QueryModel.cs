using Newtonsoft.Json;

namespace JewishBot.WebHookHanders.Telegram.Services.Yahoo
{
    public class QueryModel
    {
        [JsonProperty("query")]
        public Query Query { get; set; }
    }

    public class Query
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("results")]
        public Results Results { get; set; }
    }

    public class Results
    {
        [JsonProperty("rate")]
        public Rate Rate { get; set; }
    }

    public class Rate
    {
        [JsonProperty("rate")]
        public string _Rate { get; set; }
    }
}
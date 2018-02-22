namespace JewishBot.WebHookHandlers.Telegram.Services.UrbanDictionary
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class QueryModel
    {
        [JsonProperty("result_type")]
        public string ResultType { get; set; }

        [JsonProperty("list")]
        public List<List> List { get; set; }

        [JsonProperty("errors")]
        public Errors Errors { get; set; }
    }
}
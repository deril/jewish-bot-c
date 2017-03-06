using System.Collections.Generic;
using Newtonsoft.Json;

namespace JewishBot.WebHookHandlers.Telegram.Services.UrbanDictionary
{
    public class QueryModel
    {
        [JsonProperty("result_type")]
        public string ResultType { get; set; }

        [JsonProperty("list")]
        public List<List> List { get; set; }

        [JsonProperty("errors")]
        public Errors Errors { get; set; }
    }

    public class List
    {
        [JsonProperty("definition")]
        public string Definition { get; set; }
    }

    public class Errors
    {
        [JsonProperty("term")]
        public string Term { get; set; }
    }
}
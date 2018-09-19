namespace JewishBot.WebHookHandlers.Services.UrbanDictionary
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class QueryModel
    {
        [JsonProperty("list")]
        public IList<List> List { get; set; }

        [JsonProperty("errors")]
        public string Errors { get; set; }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JewishBot.WebHookHandlers.Telegram.Services.Poem
{
    public class QueryModel
    {
        [JsonProperty("hashid")]
        public string Hashid { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("lines")]
        public List<string> Lines { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
namespace JewishBot.WebHookHandlers.Services.Poem
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class QueryModel
    {
        [JsonProperty("hashid")]
        public string HashId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("lines")]
        public List<string> Lines { get; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
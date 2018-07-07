namespace JewishBot.WebHookHandlers.Services.DuckDuckGo
{
    using Newtonsoft.Json;

    public class RelatedTopic
    {
        [JsonProperty("Text")]
        public string Text { get; set; }
    }
}
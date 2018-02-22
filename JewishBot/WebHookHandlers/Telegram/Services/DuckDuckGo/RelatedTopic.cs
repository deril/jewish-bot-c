namespace JewishBot.WebHookHandlers.Telegram.Services.DuckDuckGo
{
    using Newtonsoft.Json;

    public class RelatedTopic
    {
        [JsonProperty("Text")]
        public string Text { get; set; }
    }
}
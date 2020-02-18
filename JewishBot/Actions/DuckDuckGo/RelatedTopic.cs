namespace JewishBot.Actions.DuckDuckGo
{
    using Newtonsoft.Json;

    public class RelatedTopic
    {
        [JsonProperty("Text")]
        public string Text { get; set; }
    }
}
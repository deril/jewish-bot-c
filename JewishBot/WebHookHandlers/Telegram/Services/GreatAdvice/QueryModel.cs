namespace JewishBot.WebHookHandlers.Telegram.Services.GreatAdvice
{
    using Newtonsoft.Json;

    public class QueryModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
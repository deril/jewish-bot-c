using Newtonsoft.Json;

namespace JewishBot.WebHookHandlers.Telegram.Services.GreatAdvice
{
    public class QueryModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
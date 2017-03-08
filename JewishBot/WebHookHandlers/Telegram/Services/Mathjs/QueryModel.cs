using Newtonsoft.Json;

namespace JewishBot.WebHookHandlers.Telegram.Services.Mathjs
{
    public class QueryModel
    {
        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }
    }
}
namespace JewishBot.WebHookHandlers.Telegram.Services.UrbanDictionary
{
    using Newtonsoft.Json;

    public class Errors
    {
        [JsonProperty("term")]
        public string Term { get; set; }
    }
}

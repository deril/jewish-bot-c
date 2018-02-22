namespace JewishBot.WebHookHandlers.Telegram.Services.GoogleMaps
{
    using Newtonsoft.Json;

    public class Result
    {
        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
    }
}
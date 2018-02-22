namespace JewishBot.WebHookHandlers.Telegram.Services.GoogleMaps
{
    using Newtonsoft.Json;

    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }
    }
}
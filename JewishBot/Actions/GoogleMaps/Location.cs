namespace JewishBot.Actions.GoogleMaps
{
    using Newtonsoft.Json;

    public class Location
    {
        [JsonProperty("lat")]
        public float Lattitude { get; set; }

        [JsonProperty("lng")]
        public float Longtitude { get; set; }
    }
}
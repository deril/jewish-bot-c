namespace JewishBot.Actions.UrbanDictionary
{
    using Newtonsoft.Json;

    public class List
    {
        [JsonProperty("definition")]
        public string Definition { get; set; }
    }
}

namespace JewishBot.WebHookHandlers.Services.Mathjs
{
    using Newtonsoft.Json;

    public class QueryModel
    {
        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }
    }
}
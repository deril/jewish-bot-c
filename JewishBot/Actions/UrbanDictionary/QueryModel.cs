using System.Collections.Generic;
using Newtonsoft.Json;

namespace JewishBot.Actions.UrbanDictionary;

public class QueryModel
{
    [JsonProperty("list")] public IList<List>? List { get; set; }

    [JsonProperty("errors")] public string? Errors { get; set; }
}

public class List
{
    [JsonProperty("definition")] public string? Definition { get; set; }
}
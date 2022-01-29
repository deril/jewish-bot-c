using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JewishBot.Actions.DuckDuckGo;

public class QueryModel
{
    [JsonProperty("AbstractText")] public string? AbstractText { get; set; }

    [JsonProperty("Type")] public string Type { get; set; }

    [JsonProperty("AbstractURL")] public Uri AbstractUrl { get; set; }

    [JsonProperty("Redirect")] public string? Redirect { get; set; }

    [JsonProperty("RelatedTopics")] public IList<RelatedTopic> RelatedTopics { get; set; }
}

public class RelatedTopic
{
    [JsonProperty("Text")] public string? Text { get; set; }
}
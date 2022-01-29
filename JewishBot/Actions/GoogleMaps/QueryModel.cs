using System.Collections.Generic;
using Newtonsoft.Json;

namespace JewishBot.Actions.GoogleMaps;

public class QueryModel
{
    [JsonProperty("results")] public IList<Result>? Results { get; set; }

    [JsonProperty("status")] public string? Status { get; set; }
}

public class Result
{
    [JsonProperty("geometry")] public Geometry? Geometry { get; set; }
}

public class Geometry
{
    [JsonProperty("location")] public Location? Location { get; set; }
}

public class Location
{
    [JsonProperty("lat")] public float Lattitude { get; set; }

    [JsonProperty("lng")] public float Longtitude { get; set; }
}
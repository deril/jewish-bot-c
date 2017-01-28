using System.Collections.Generic;
using Newtonsoft.Json;

namespace JewishBot.Services.GoogleMaps
{
    public class QueryModel
    {
        [JsonProperty("results")]
        public List<Result> Results;

        [JsonProperty("status")]
        public string Status;
    }

    public class Result
    {
        [JsonProperty("geometry")]
        public Geometry Geometry;
    }

    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location;
    }

    public class Location
    {
        [JsonProperty("lat")]
        public float Lattitude;

        [JsonProperty("lng")]
        public float Longtitude;
    }
}
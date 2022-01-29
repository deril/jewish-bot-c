using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace JewishBot.Actions.GoogleMaps;

public class GoogleMapsApi
{
    private readonly string _apiKey;
    private readonly IHttpClientFactory _clientFactory;

    public GoogleMapsApi(IHttpClientFactory clientFactory, string apiKey)
    {
        _clientFactory = clientFactory;
        _apiKey = apiKey;
    }

    public async Task<QueryModel> InvokeAsync(IEnumerable<string> arguments)
    {
        var client = _clientFactory.CreateClient("googleapis");
        if (client.BaseAddress is null) return new QueryModel();

        var query = new Dictionary<string, string?>
        {
            {"address", string.Join(string.Empty, arguments)},
            {"key", _apiKey}
        };
        var route = new UriBuilder(client.BaseAddress)
        {
            Path = "maps/api/geocode/json"
        };

        try
        {
            var response =
                await client.GetStringAsync(new Uri(QueryHelpers.AddQueryString(route.Uri.ToString(), query)));
            return JsonConvert.DeserializeObject<QueryModel>(response) ?? new QueryModel();
        }
        catch (HttpRequestException)
        {
            return new QueryModel();
        }
    }
}
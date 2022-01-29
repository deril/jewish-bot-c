using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.Actions.Weather;

public class WeatherApi
{
    private readonly IHttpClientFactory _clientFactory;

    public WeatherApi(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<string> InvokeAsync(IEnumerable<string> arguments)
    {
        var client = _clientFactory.CreateClient("weatherapi");
        if (client.BaseAddress is null) return string.Empty;

        var query = new Dictionary<string, string?>
        {
            {"format", "3"}
        };
        var route = new UriBuilder(client.BaseAddress)
        {
            Path = string.Join("+", arguments)
        };
        try
        {
            var response =
                await client.GetStringAsync(new Uri(QueryHelpers.AddQueryString(route.Uri.ToString(), query)));
            return response;
        }
        catch (HttpRequestException)
        {
            return string.Empty;
        }
    }
}
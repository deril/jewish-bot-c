using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace JewishBot.Actions.DuckDuckGo;

public class GoApi
{
    private readonly IHttpClientFactory _clientFactory;

    public GoApi(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<QueryModel> InvokeAsync(IEnumerable<string> arguments)
    {
        var client = _clientFactory.CreateClient("duckduckgo");
        if (client.BaseAddress is null) return new QueryModel();

        var query = new Dictionary<string, string?>
        {
            {"q", string.Join(string.Empty, arguments)},
            {"format", "json"}
        };

        try
        {
            var response =
                await client.GetStringAsync(
                    new Uri(QueryHelpers.AddQueryString(client.BaseAddress.ToString(), query)));
            return JsonConvert.DeserializeObject<QueryModel>(response) ?? new QueryModel();
        }
        catch (HttpRequestException e)
        {
            return new QueryModel();
        }
    }
}
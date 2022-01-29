using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace JewishBot.Actions.UrbanDictionary;

public class DictApi
{
    private readonly IHttpClientFactory _clientFactory;

    public DictApi(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<QueryModel> InvokeAsync(IEnumerable<string> arguments)
    {
        var client = _clientFactory.CreateClient("urbandictionary");
        var query = new Dictionary<string, string>
        {
            {"term", string.Join(" ", arguments)}
        };
        var route = new UriBuilder(client.BaseAddress)
        {
            Path = "v0/define"
        };

        try
        {
            var response =
                await client.GetStringAsync(new Uri(QueryHelpers.AddQueryString(route.Uri.ToString(), query)));
            return JsonConvert.DeserializeObject<QueryModel>(response);
        }
        catch (HttpRequestException e)
        {
            return new QueryModel();
        }
    }
}
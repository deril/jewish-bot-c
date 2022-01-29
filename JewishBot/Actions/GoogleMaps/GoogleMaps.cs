using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram;

namespace JewishBot.Actions.GoogleMaps;

internal class GoogleMaps : IAction
{
    private readonly string _apiKey;
    private readonly IReadOnlyCollection<string> _args;
    private readonly IBotService _botService;
    private readonly long _chatId;
    private readonly IHttpClientFactory _clientFactory;

    public GoogleMaps(IBotService botService, IHttpClientFactory clientFactory, long chatId,
        IReadOnlyCollection<string> args, string key)
    {
        _botService = botService;
        _clientFactory = clientFactory;
        _chatId = chatId;
        _args = args;
        _apiKey = key;
    }

    public async Task HandleAsync()
    {
        var message = "Please specify an address";
        if (_args.Count == 0)
        {
            await _botService.Client.SendTextMessageAsync(_chatId, message);
            return;
        }

        var mapsApi = new GoogleMapsApi(_clientFactory, _apiKey);
        var response = await mapsApi.InvokeAsync(_args);

        var geometryLocation = response.Results?[0].Geometry?.Location;

        if (response.Status != "OK" || geometryLocation is null)
        {
            message = "Nothing \uD83D\uDE22";
            await _botService.Client.SendTextMessageAsync(_chatId, message);
            return;
        }

        await _botService.Client.SendLocationAsync(_chatId, geometryLocation.Lattitude, geometryLocation.Longtitude);
    }
}
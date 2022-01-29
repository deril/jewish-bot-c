using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram;

namespace JewishBot.Actions.Weather;

internal class Weather : IAction
{
    private readonly IReadOnlyCollection<string> _args;
    private readonly IBotService _botService;
    private readonly long _chatId;
    private readonly IHttpClientFactory _clientFactory;

    public Weather(IBotService botService, IHttpClientFactory clientFactory, long chatId,
        IReadOnlyCollection<string> args)
    {
        _botService = botService;
        _clientFactory = clientFactory;
        _chatId = chatId;
        _args = args;
    }

    public async Task HandleAsync()
    {
        if (_args.Count == 0)
        {
            await _botService.SendMessageAsync("Please specify a city or region", _chatId);
            return;
        }

        var weatherApi = new WeatherApi(_clientFactory);
        var response = await weatherApi.InvokeAsync(_args);
        if (string.IsNullOrEmpty(response))
        {
            await _botService.SendMessageAsync("Nothing \uD83D\uDE22", _chatId);
            return;
        }

        await _botService.SendMessageAsync(response, _chatId);
    }
}
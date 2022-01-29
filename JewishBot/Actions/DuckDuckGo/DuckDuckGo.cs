using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram;

namespace JewishBot.Actions.DuckDuckGo;

internal class DuckDuckGo : IAction
{
    private readonly IReadOnlyCollection<string> _args;
    private readonly IBotService _botService;
    private readonly long _chatId;
    private readonly IHttpClientFactory _clientFactory;

    public DuckDuckGo(IBotService botService, IHttpClientFactory clientFactory, long chatId,
        IReadOnlyCollection<string> args)
    {
        _botService = botService;
        _clientFactory = clientFactory;
        _chatId = chatId;
        _args = args;
    }

    public async Task HandleAsync()
    {
        var message = "Please specify at least 1 search term";
        if (_args.Count != 0)
        {
            var go = new GoApi(_clientFactory);
            var result = await go.InvokeAsync(_args);
            message = result.Type switch
            {
                "A" => result.AbstractText,
                "D" => result.RelatedTopics?[0].Text,
                "E" => result.Redirect,
                "C" => result.AbstractUrl?.ToString(),
                _ => "Nothing found \uD83D\uDE22"
            };
        }

        await _botService.SendMessageAsync(message ?? "Nothing found \uD83D\uDE22", _chatId);
    }
}
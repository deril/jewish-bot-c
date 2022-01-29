using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram;

namespace JewishBot.Actions;

internal class Hey : IAction
{
    private readonly IBotService _botService;
    private readonly long _chatId;

    public Hey(IBotService botService, long chatId)
    {
        _botService = botService;
        _chatId = chatId;
    }

    public async Task HandleAsync()
    {
        await _botService.SendMessageAsync("היי! (Hey)", _chatId);
    }
}
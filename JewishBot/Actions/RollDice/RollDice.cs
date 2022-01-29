using System.Collections.ObjectModel;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram;

namespace JewishBot.Actions.RollDice;

internal class RollDice : IAction
{
    private readonly ReadOnlyCollection<string> _args;
    private readonly IBotService _botService;
    private readonly long _chatId;
    private readonly string _username;

    public RollDice(IBotService botService, long chatId, ReadOnlyCollection<string> args, string username)
    {
        _botService = botService;
        _chatId = chatId;
        _args = args;
        _username = username;
    }

    public async Task HandleAsync()
    {
        if (!IsParsableArguments())
        {
            await _botService.Client.SendDiceAsync(_chatId);
            return;
        }

        var result = new Dice(_args[0]);
        var message = result.GetSum();

        await _botService.Client.SendTextMessageAsync(_chatId, $"{_username}: \uD83C\uDFB2 {message}");
    }

    private bool IsParsableArguments()
    {
        return IsArgumentsPassed() && Dice.CanParse(_args[0]);
    }

    private bool IsArgumentsPassed()
    {
        return _args.Count != 0;
    }
}
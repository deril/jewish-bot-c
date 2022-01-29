using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Api.Forex.Sharp;
using JewishBot.WebHookHandlers.Telegram;

namespace JewishBot.Actions;

internal class CurrencyExchange : IAction
{
    private const string Description = @"Converts currencies using Yahoo API; default amount 1.
Usage: /ex [amount] [fromCurrency] in [toCurrency];
/ex [fromCurrency] [toCurrency]";

    private readonly string _apiKey;
    private readonly ReadOnlyCollection<string> _args;

    private readonly IBotService _botService;
    private readonly long _chatId;

    public CurrencyExchange(IBotService botService, long chatId, ReadOnlyCollection<string> args, string key)
    {
        _botService = botService;
        _chatId = chatId;
        _args = args;
        _apiKey = key;
    }

    public async Task HandleAsync()
    {
        var message = PrepareMessage();
        await _botService.Client.SendTextMessageAsync(_chatId, message);
    }

    private string PrepareMessage()
    {
        string fromCurrency;
        string toCurrency;
        decimal amount = 0;

        if (IsInvalidArguments()) return Description;

        switch (_args.Count)
        {
            case 2:
                fromCurrency = _args[0];
                toCurrency = _args[1];
                break;
            case 4:
                if (!decimal.TryParse(_args[0], out amount)) amount = 0;

                fromCurrency = _args[1];
                toCurrency = _args[3];
                break;
            default:
                return Description;
        }

        try
        {
            var culture = new CultureInfo("uk-UA", true);
            var financeApi = ApiForex.GetRate(_apiKey).Result;
            var value = financeApi.Convert(toCurrency.ToUpper(culture), fromCurrency.ToUpper(culture), amount);
            return $"{amount} {fromCurrency} -> {value:0.00} {toCurrency}";
        }
        catch (InvalidOperationException)
        {
            // When currency wasn't found
            return $"Cannot convert from {fromCurrency} to {toCurrency}";
        }
    }

    private bool IsInvalidArguments()
    {
        return _args.Count == 0 || _args.Any(string.IsNullOrEmpty);
    }
}
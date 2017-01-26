using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using JewishBot.Services.Yahoo;
using Telegram.Bot;

namespace JewishBot.Actions
{
    internal class CurrencyExchange : IAction
    {
        public static string Description { get; } = @"Converts currencies using Yahoo API; default amount 1.
Usage: /ex [fromCurrency] [toCurrency] [amount]";

        private readonly TelegramBotClient _bot;
        private readonly CurrencyApi _currencyApi = new CurrencyApi();
        private string _message;
        private string _fromCurrency;
        private string _toCurrency;
        private decimal _amount;

        public CurrencyExchange(TelegramBotClient bot)
        {
            _bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args)
        {
            await PrepareMessageAsync(args);
            await _bot.SendTextMessageAsync(chatId, _message);
        }

        private async Task PrepareMessageAsync(IReadOnlyList<string> args)
        {
            if (args == null || args.Any(argument => argument == null))
            {
                _message = Description;
                return;
            }

            switch (args.Count)
            {
                case 2:
                    _fromCurrency = args[0];
                    _toCurrency = args[1];
                    break;
                case 3:
                    _fromCurrency = args[0];
                    _toCurrency = args[1];
                    _amount = decimal.Parse(args[2]);
                    break;
                default:
                    _message = Description;
                    return;
            }

            if (_fromCurrency != null && _toCurrency != null)
            {
                var rates = await _currencyApi.Invoke<QueryModel>($"{_fromCurrency}{_toCurrency}");

                if (rates.Query == null || rates.Query.Results.Rate._Rate == "N/A")
                {
                    _message = "Something goes wrong";
                    return;
                }

                var rateResult = rates.Query.Results.Rate;

                if (_amount == 0)
                {
                    _message = $"{_fromCurrency}/{_toCurrency} -> {rateResult._Rate}";
                }
                else
                {
                    var value = decimal.Parse(rateResult._Rate, new CultureInfo(rates.Query.Lang)) * _amount;
                    _message = $"{_amount} {_fromCurrency} -> {value:0.00} {_toCurrency}";
                }
            }
        }
    }
}
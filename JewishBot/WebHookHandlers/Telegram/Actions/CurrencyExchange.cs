using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.Yahoo;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    class CurrencyExchange : IAction
    {
        public static string Description { get; } = @"Converts currencies using Yahoo API; default amount 1.
Usage: /ex [fromCurrency] [toCurrency] [amount]";

        TelegramBotClient Bot { get; }
        long ChatId { get; }
        string[] Args { get; }

        public CurrencyExchange(TelegramBotClient bot, long chatId, string[] args)
        {
            Bot = bot;
            ChatId = chatId;
            Args = args;
        }

        public async Task HandleAsync()
        {
            var message = await PrepareMessageAsync();
            await Bot.SendTextMessageAsync(ChatId, message);
        }

        async Task<string> PrepareMessageAsync()
        {
            string fromCurrency;
            string toCurrency;
            decimal amount = 0;

            if (Args == null || Args.Any(argument => argument == null))
            {
                return Description;
            }

            switch (Args.Length)
            {
                case 2:
                    fromCurrency = Args[0];
                    toCurrency = Args[1];
                    break;
                case 3:
                    fromCurrency = Args[0];
                    toCurrency = Args[1];
                    decimal.TryParse(Args[2], out amount);
                    break;
                default:
                    return Description;
            }

            if (fromCurrency == null || toCurrency == null) return Description;
            var currencyApi = new CurrencyApi();
            var rates = await currencyApi.Invoke<QueryModel>($"{fromCurrency}{toCurrency}");

            if (rates.Query?.Results?.Rate?._Rate == null || rates.Query.Results.Rate._Rate == "N/A")
            {
                return "Something goes wrong";
            }

            var rateResult = rates.Query.Results.Rate;

            if (amount == 0)
            {
                return $"{fromCurrency}/{toCurrency} -> {rateResult._Rate}";
            }

            var value = decimal.Parse(rateResult._Rate, new CultureInfo(rates.Query.Lang)) * amount;
            return $"{amount} {fromCurrency} -> {value:0.00} {toCurrency}";
        }
    }
}
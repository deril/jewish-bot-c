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

        private TelegramBotClient Bot { get; }

        public CurrencyExchange(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args)
        {
            var message = await PrepareMessageAsync(args);
            await Bot.SendTextMessageAsync(chatId, message);
        }

        private static async Task<string> PrepareMessageAsync(IReadOnlyList<string> args)
        {
            string fromCurrency;
            string toCurrency;
            decimal amount;

            if (args == null || args.Any(argument => argument == null))
            {
                return Description;
            }

            switch (args.Count)
            {
                case 2:
                    fromCurrency = args[0];
                    toCurrency = args[1];
                    break;
                case 3:
                    fromCurrency = args[0];
                    toCurrency = args[1];
                    amount = decimal.Parse(args[2]);
                    break;
                default:
                    return Description;
            }

            if (fromCurrency != null && toCurrency != null)
            {
                var currencyApi = new CurrencyApi();
                var rates = await currencyApi.Invoke<QueryModel>($"{fromCurrency}{toCurrency}");

                if (rates.Query == null || rates.Query.Results.Rate._Rate == "N/A")
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
}
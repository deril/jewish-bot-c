using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using JewishBot.Services.Yahoo;
using Telegram.Bot;

namespace JewishBot.Actions
{
    class CurrencyExchange : IAction {

        public static string Description { get; } = @"Converts currencies using Yahoo API; default amount 1. 
Usage: /ex [fromCurrency] [toCurrency] [amount]";
        private readonly TelegramBotClient bot;
        private readonly CurrencyApi currencyApi = new CurrencyApi();
        private string message;
        private string fromCurrency;
        private string toCurrency;
        private decimal amount = 0;

        public CurrencyExchange(TelegramBotClient bot) {
            this.bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args) {
            await PrepareMessageAsync(args);
            await bot.SendTextMessageAsync(chatId, message);
        }

        private async Task PrepareMessageAsync(string[] args) {
            if (args.Any(argument => argument == null)) {
                message = CurrencyExchange.Description;
                return;
            }

            switch (args.Length) {
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
                    message = CurrencyExchange.Description;
                    return;
            }

            if (fromCurrency != null && toCurrency != null) {
                var rates = await currencyApi.Invoke<QueryModel>($"{fromCurrency}{toCurrency}");

                if (rates.Query == null || rates.Query.Results.Rate._Rate == "N/A") {
                    message = "Something goes wrong";
                    return;
                }

                var rateResult = rates.Query.Results.Rate;

                if (amount == 0) {
                    message = $"{fromCurrency}/{toCurrency} -> {rateResult._Rate}";
                } else {
                    decimal value = decimal.Parse(rateResult._Rate, new CultureInfo(rates.Query.Lang)) * amount;
                    message = String.Format("{0} {1} -> {2:0.00} {3}", amount, fromCurrency, value, toCurrency);
                }
            }
        }
    }
}
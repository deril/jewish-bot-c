namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Forex.Sharp;
    using Api.Forex.Sharp.Models;
    using global::Telegram.Bot;

    internal class CurrencyExchange : IAction
    {
        private readonly ITelegramBotClient bot;
        private readonly long chatId;
        private readonly string[] args;
        private readonly ApiForexRates financeApi;

        public CurrencyExchange(ITelegramBotClient bot, long chatId, string[] args, string key)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
            this.financeApi = ApiForex.GetRate(key).Result;
        }

        public static string Description { get; } = @"Converts currencies using Yahoo API; default amount 1.
Usage: /ex [amount] [fromCurrency] in [toCurrency];
/ex [fromCurrency] [toCurrency]";

        public async Task HandleAsync()
        {
            var message = this.PrepareMessage();
            await this.bot.SendTextMessageAsync(this.chatId, message);
        }

        private string PrepareMessage()
        {
            string fromCurrency;
            string toCurrency;
            decimal amount = 0;

            if (this.args == null || this.args.Any(argument => argument == null))
            {
                return Description;
            }

            switch (this.args.Length)
            {
                case 2:
                    fromCurrency = this.args[0];
                    toCurrency = this.args[1];
                    break;
                case 4:
                    decimal.TryParse(this.args[0], out amount);
                    fromCurrency = this.args[1];
                    toCurrency = this.args[3];
                    break;
                default:
                    return Description;
            }

            if (fromCurrency == null || toCurrency == null)
            {
                return Description;
            }

            try
            {
                var value = this.financeApi.Convert(toCurrency.ToUpper(), fromCurrency.ToUpper(), amount);
                return $"{amount} {fromCurrency} -> {value:0.00} {toCurrency}";
            }
            catch (InvalidOperationException)
            {
                // When currency wasn't found
                return $"Cannot convert from {fromCurrency} to {toCurrency}";
            }
        }
    }
}
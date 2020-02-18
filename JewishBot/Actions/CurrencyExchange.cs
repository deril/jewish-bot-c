namespace JewishBot.Actions
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Forex.Sharp;
    using WebHookHandlers.Telegram;

    internal class CurrencyExchange : IAction
    {
        private const string Description = @"Converts currencies using Yahoo API; default amount 1.
Usage: /ex [amount] [fromCurrency] in [toCurrency];
/ex [fromCurrency] [toCurrency]";

        private readonly string apiKey;
        private readonly ReadOnlyCollection<string> args;

        private readonly IBotService botService;
        private readonly long chatId;

        public CurrencyExchange(IBotService botService, long chatId, ReadOnlyCollection<string> args, string key)
        {
            this.botService = botService;
            this.chatId = chatId;
            this.args = args;
            this.apiKey = key;
        }

        public async Task HandleAsync()
        {
            var message = this.PrepareMessage();
            await this.botService.Client.SendTextMessageAsync(this.chatId, message);
        }

        private string PrepareMessage()
        {
            string fromCurrency;
            string toCurrency;
            decimal amount = 0;

            if (this.IsInvalidArguments())
            {
                return Description;
            }

            switch (this.args.Count)
            {
                case 2:
                    fromCurrency = this.args[0];
                    toCurrency = this.args[1];
                    break;
                case 4:
                    if (!decimal.TryParse(this.args[0], out amount))
                    {
                        amount = 0;
                    }

                    fromCurrency = this.args[1];
                    toCurrency = this.args[3];
                    break;
                default:
                    return Description;
            }

            try
            {
                var culture = new CultureInfo("uk-UA", true);
                var financeApi = ApiForex.GetRate(this.apiKey).Result;
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
            return this.args == null || this.args.Any(argument => argument == null);
        }
    }
}
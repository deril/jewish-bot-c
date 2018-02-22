namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Services;

    internal class CurrencyExchange : IAction
    {
        private readonly ITelegramBotClient bot;
        private readonly long chatId;
        private readonly string[] args;
        private readonly ApiService financeApi;

        public CurrencyExchange(ITelegramBotClient bot, long chatId, string[] args, ApiService apiService)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
            this.financeApi = apiService;
        }

        public static string Description { get; } = @"Converts currencies using Yahoo API; default amount 1.
Usage: /ex [amount] [fromCurrency] in [toCurrency];
/ex [fromCurrency] [toCurrency]";

        public async Task HandleAsync()
        {
            var message = await this.PrepareMessageAsync();
            await this.bot.SendTextMessageAsync(this.chatId, message);
        }

        private async Task<string> PrepareMessageAsync()
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

            var rate = await this.financeApi.InvokeAsync(new string[] { amount.ToString(), fromCurrency, toCurrency });
            var value = decimal.Parse(rate, CultureInfo.InvariantCulture);

            return $"{amount} {fromCurrency} -> {value:0.00} {toCurrency}";
        }
    }
}
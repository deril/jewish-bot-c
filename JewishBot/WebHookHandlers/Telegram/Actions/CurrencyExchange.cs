using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    class CurrencyExchange : IAction
    {
        public static string Description { get; } = @"Converts currencies using Yahoo API; default amount 1.
Usage: /ex [amount] [fromCurrency] in [toCurrency];
/ex [fromCurrency] [toCurrency]";

        ITelegramBotClient Bot { get; }
        long ChatId { get; }
        string[] Args { get; }
        ApiService FinanceApi { get; }

        public CurrencyExchange(ITelegramBotClient bot, long chatId, string[] args, ApiService apiService)
        {
            Bot = bot;
            ChatId = chatId;
            Args = args;
            FinanceApi = apiService;
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
                case 4:
                    decimal.TryParse(Args[0], out amount);
                    fromCurrency = Args[1];
                    toCurrency = Args[3];
                    break;
                default:
                    return Description;
            }

            if (fromCurrency == null || toCurrency == null) return Description;
            var rate = await FinanceApi.InvokeAsync(new string[] { amount.ToString(), fromCurrency, toCurrency });
            var value = decimal.Parse(rate, CultureInfo.InvariantCulture);

            return $"{amount} {fromCurrency} -> {value:0.00} {toCurrency}";
        }
    }
}
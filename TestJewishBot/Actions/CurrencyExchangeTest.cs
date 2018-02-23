using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Actions;
using JewishBot.WebHookHandlers.Telegram.Services;
using JewishBot.WebHookHandlers.Telegram.Services.GoogleFinance;
using Moq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace TestJewishBot
{
    public class CurrencyExchangeTest
    {
        [Fact]
        public void MakeValidExchangeAsync()
        {
            var bot = new Mock<ITelegramBotClient>();
            long chatId = 42;
            string amount = "42", fromCurrency = "XXX", toCurrency = "UAH", rate = "1166.9070";
            var expectedResult = $"{amount} {fromCurrency} -> 1166.91 {toCurrency}";
            var externalArgs = new string[] { amount, fromCurrency, "in", toCurrency };
            var financeArgs = new string[] { amount, fromCurrency, toCurrency };
            var financeApi = new Mock<FinanceApi>();
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            bot.Setup(b => b.SendTextMessageAsync(chatId, expectedResult, 0, false, false, 0, null, source.Token)).Verifiable();
            financeApi.Setup(f => f.InvokeAsync(financeArgs)).ReturnsAsync(rate);
            IAction currencyExchange = new CurrencyExchange(bot.Object, chatId, externalArgs, financeApi.Object);

            currencyExchange.HandleAsync().GetAwaiter();

            bot.Verify(m => m.SendTextMessageAsync(chatId, expectedResult, 0, false, false, 0, null, source.Token));
            financeApi.VerifyAll();
        }
    }
}

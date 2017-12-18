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
        public async void MakeValidExchangeAsync()
        {
            var bot = new Mock<ITelegramBotClient>(MockBehavior.Strict);
            ChatId actualChatId = 0;
            long chatId = 42;
            string amount = "42", fromCurrency = "XXX", toCurrency = "UAH", rate = "1166.9070";
            var expectedResult = $"{amount} {fromCurrency} -> 1166.91 {toCurrency}";
            var externalArgs = new string[] { amount, fromCurrency, "in", toCurrency };
            var financeArgs = new string[] { amount, fromCurrency, toCurrency };
            var financeApi = new Mock<FinanceApi>();
            bot.Setup(b => b.SendTextMessageAsync(chatId, expectedResult, 0, false, false, 0, null, It.IsAny<CancellationToken>())).Verifiable();
            financeApi.Setup(f => f.InvokeAsync(financeArgs)).ReturnsAsync(rate);
            IAction currencyExchange = new CurrencyExchange(bot.Object, chatId, externalArgs, financeApi.Object);

            await currencyExchange.HandleAsync();

            bot.Verify(m => m.SendTextMessageAsync(chatId, expectedResult, 0, false, false, 0, null, It.IsAny<CancellationToken>()));
            financeApi.VerifyAll();
        }
    }
}

using System.Net;
using JewishBot.Services.GreatAdvice;
using Telegram.Bot;

namespace JewishBot.Actions
{
    public class Advice : IAction
    {
        private TelegramBotClient Bot { get; }

        public Advice(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async void HandleAsync(long chatId, string username)
        {
            var adviceService = new GreatAdviceApi();
            var result = await adviceService.Invoke<QueryModel>(null);
            var textResult = WebUtility.HtmlDecode(result.Text);
            var message = $"Advice for {username}: {textResult}";

            await Bot.SendTextMessageAsync(chatId, message);
        }
    }
}
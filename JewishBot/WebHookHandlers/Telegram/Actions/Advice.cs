using System.Net;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.GreatAdvice;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    public class Advice : IAction
    {
		TelegramBotClient Bot { get; }

		public Advice(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async Task HandleAsync(long chatId, string username)
        {
            var adviceService = new GreatAdviceApi();
            var result = await adviceService.Invoke<QueryModel>(null);
            var textResult = WebUtility.HtmlDecode(result.Text);
            var message = $"Advice for {username}: {textResult}";

            await Bot.SendTextMessageAsync(chatId, message);
        }
    }
}
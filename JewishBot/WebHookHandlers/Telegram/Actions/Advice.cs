using System.Net;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.GreatAdvice;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    public class Advice : IAction
    {
        TelegramBotClient Bot { get; }
        long ChatId { get; }
        string Username { get; }

        public Advice(TelegramBotClient bot, long chatId, string username)
        {
            Bot = bot;
            ChatId = chatId;
            Username = username;
        }

        public async Task HandleAsync()
        {
            var adviceService = new GreatAdviceApi();
            var result = await adviceService.Invoke<QueryModel>(null);
            var textResult = WebUtility.HtmlDecode(result.Text);
            var message = $"Advice for {Username}: {textResult}";

            await Bot.SendTextMessageAsync(ChatId, message);
        }
    }
}
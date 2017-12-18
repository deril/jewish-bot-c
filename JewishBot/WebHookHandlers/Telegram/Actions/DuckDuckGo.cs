using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.DuckDuckGo;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    class DuckDuckGo : IAction
    {
        TelegramBotClient Bot { get; }
        long ChatId { get; }
        string[] Args { get; }

        public DuckDuckGo(TelegramBotClient bot, long chatId, string[] args)
        {
            Bot = bot;
            ChatId = chatId;
            Args = args;
        }

        public async Task HandleAsync()
        {
            var message = "Please specify at least 1 search term";
            if (Args != null)
            {
                var go = new GoApi();
                var result = await go.InvokeAsync<QueryModel>(new string[] { string.Join(" ", Args) });
                switch (result.Type)
                {
                    case "A":
                        message = result.AbstractText;
                        break;
                    case "D":
                        message = result.RelatedTopics[0].Text;
                        break;
                    case "E":
                        message = result.Redirect;
                        break;
                    case "C":
                        message = result.AbstractUrl;
                        break;
                    default:
                        // TODO: implement here logging
                        message = "Nothing found \uD83D\uDE22";
                        break;
                }
            }
            await Bot.SendTextMessageAsync(ChatId, message);
        }
    }
}

using JewishBot.Services.DuckDuckGo;
using Telegram.Bot;

namespace JewishBot.Actions
{
    internal class DuckDuckGo : IAction
    {
        private TelegramBotClient Bot { get; }

        public DuckDuckGo(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args)
        {
            var message = "Please specify at least 1 search term";
            if (args != null)
            {
                var go = new GoApi();
                var result = await go.Invoke<QueryModel>(string.Join(" ", args));
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
            await Bot.SendTextMessageAsync(chatId, message);
        }
    }
}

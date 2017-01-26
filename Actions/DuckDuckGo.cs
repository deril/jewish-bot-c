using JewishBot.Services.DuckDuckGo;
using Telegram.Bot;

namespace JewishBot.Actions
{
    internal class DuckDuckGo : IAction
    {
        private readonly TelegramBotClient _bot;
        private readonly GoApi _go = new GoApi();
        private string _message = "Please specify at least 1 search term";

        public DuckDuckGo(TelegramBotClient bot)
        {
            _bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args)
        {
            if (args != null)
            {
                var result = await _go.Invoke<QueryModel>(string.Join(" ", args));
                switch (result.Type)
                {
                    case "A":
                        _message = result.AbstractText;
                        break;
                    case "D":
                        _message = result.RelatedTopics[0].Text;
                        break;
                    case "E":
                        _message = result.Redirect;
                        break;
                    case "C":
                        _message = result.AbstractUrl;
                        break;
                    default:
                        // TODO: implement here logging
                        _message = "Nothing found \uD83D\uDE22";
                        break;
                }
            }
            await _bot.SendTextMessageAsync(chatId, _message);
        }
    }
}
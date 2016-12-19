using JewishBot.Services.DuckDuckGo;
using Telegram.Bot;

namespace JewishBot.Actions {
    public class DuckDuckGo : IAction {

        private readonly TelegramBotClient bot;
        private readonly GoApi Go = new GoApi();
        private string message = "Please cpecify at least 1 search term";

        public DuckDuckGo(TelegramBotClient bot) {
            this.bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args) {
            if (args != null) {
                var result = await Go.Invoke<QueryModel>(string.Join(" ", args));
                switch (result.Type) {
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
                        message = result.AbstractURL;
                        break;
                    default:
                        // TODO: implement here logging
                        message = "Nothing found \uD83D\uDE22";
                        break;
                }
            }
            await bot.SendTextMessageAsync(chatId, message);
        }
    }
}
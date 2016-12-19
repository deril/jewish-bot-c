using JewishBot.Services.UrbanDictionary;
using Telegram.Bot;

namespace JewishBot.Actions {
    public class UrbanDictionary : IAction {

        private readonly TelegramBotClient bot;
        private readonly DictApi UD = new DictApi();
        private string message = "Please cpecify at least 1 search term";

        public UrbanDictionary(TelegramBotClient bot) {
            this.bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args) {
            if (args != null) {
                var result = await UD.Invoke<QueryModel>(string.Join(" ", args));
                if (result.ResultType == "exact") {
                    message = result.List[0].Definition;
                } else {
                    message = "Nothing found \uD83D\uDE22";
                    // TODO: implement here logging
                }
            }
            await bot.SendTextMessageAsync(chatId, message);
        }
    }
}
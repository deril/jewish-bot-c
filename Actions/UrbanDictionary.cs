using JewishBot.Services.UrbanDictionary;
using Telegram.Bot;

namespace JewishBot.Actions
{
    internal class UrbanDictionary : IAction
    {
        private readonly TelegramBotClient _bot;
        private readonly DictApi _ud = new DictApi();
        private string _message = "Please specify at least 1 search term";

        public UrbanDictionary(TelegramBotClient bot)
        {
            _bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args)
        {
            if (args != null)
            {
                var result = await _ud.Invoke<QueryModel>(string.Join(" ", args));
                _message = result.ResultType == "exact" ? result.List[0].Definition : "Nothing found \uD83D\uDE22";
            }
            await _bot.SendTextMessageAsync(chatId, _message);
        }
    }
}
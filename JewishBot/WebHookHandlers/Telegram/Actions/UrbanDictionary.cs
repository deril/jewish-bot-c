using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.UrbanDictionary;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	class UrbanDictionary : IAction
	{
		TelegramBotClient Bot { get; }

		public UrbanDictionary(TelegramBotClient bot)
		{
			Bot = bot;
		}

		public async Task HandleAsync(long chatId, string[] args)
		{
			var message = "Please specify at least 1 search term";

			if (args != null)
			{
				var ud = new DictApi();
				var result = await ud.Invoke<QueryModel>(string.Join(" ", args));
				message = result.ResultType == "exact" ? result.List[0].Definition : "Nothing found \uD83D\uDE22";
			}
			await Bot.SendTextMessageAsync(chatId, message);
		}
	}
}
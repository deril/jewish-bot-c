using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.UrbanDictionary;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	class UrbanDictionary : IAction
	{
		TelegramBotClient Bot { get; }
		long ChatId { get; }
		string[] Args { get; }

		public UrbanDictionary(TelegramBotClient bot, long chatId, string[] args)
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
				var ud = new DictApi();
				var result = await ud.Invoke<QueryModel>(string.Join(" ", Args));
				message = result.ResultType == "exact" ? result.List[0].Definition : "Nothing found \uD83D\uDE22";
			}
			await Bot.SendTextMessageAsync(ChatId, message);
		}
	}
}
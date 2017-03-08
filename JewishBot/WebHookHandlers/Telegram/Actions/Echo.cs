using System.Threading.Tasks;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	class Echo : IAction
	{
		TelegramBotClient Bot { get; }

		public Echo(TelegramBotClient bot)
		{
			Bot = bot;
		}

		public static string Description { get; } = @"Returns input text.
            Usage: /echo <text>";

		public async Task HandleAsync(long chatId, string[] args)
		{
			if (args == null) return;
			await Bot.SendTextMessageAsync(chatId, string.Join(" ", args));
		}
	}
}

using System.Threading.Tasks;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	class Hey : IAction
	{
		TelegramBotClient Bot { get; }

		public Hey(TelegramBotClient bot)
		{
			Bot = bot;
		}

		public static string Description { get; } = @"Helloes to sender.
            Usage: /hey";

		public async Task HandleAsync(long chatId)
		{
			await Bot.SendTextMessageAsync(chatId, "Hey!");
		}
	}
}
using System.Threading.Tasks;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	class Hey : IAction
	{
		TelegramBotClient Bot { get; }
        long ChatId { get; }

		public Hey(TelegramBotClient bot, long chatId)
		{
			Bot = bot;
            ChatId = chatId;
		}

		public static string Description { get; } = @"Helloes to sender.
            Usage: /hey";

		public async Task HandleAsync()
		{
			await Bot.SendTextMessageAsync(ChatId, "!היי (Hey)");
		}
	}
}
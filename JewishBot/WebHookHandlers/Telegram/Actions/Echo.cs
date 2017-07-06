using System.Threading.Tasks;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	class Echo : IAction
	{
		TelegramBotClient Bot { get; }
        long ChatId { get; }
        string[] Args { get; }

		public Echo(TelegramBotClient bot, long chatId, string[] args)
		{
			Bot = bot;
            ChatId = chatId;
            Args = args;
		}

		public static string Description { get; } = @"Returns input text.
            Usage: /echo <text>";

		public async Task HandleAsync()
		{
			if (Args == null) return;
			await Bot.SendTextMessageAsync(ChatId, string.Join(" ", Args));
		}
	}
}

using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.DiceGame;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	class RollDice : IAction
	{
		const string DefaultPatern = "1d6";
		TelegramBotClient Bot { get; }

		public RollDice(TelegramBotClient bot)
		{
			Bot = bot;
		}

		public async Task HandleAsync(long chatId, string username, string[] args)
		{
			var toParse = (args != null && Dice.CanParse(args[0])) ? args[0] : DefaultPatern;
			var result = new Dice(toParse);

			var message = result.GetSum().ToString();

			await Bot.SendTextMessageAsync(chatId, $"{username}: \uD83C\uDFB2 {message}");
		}
	}
}
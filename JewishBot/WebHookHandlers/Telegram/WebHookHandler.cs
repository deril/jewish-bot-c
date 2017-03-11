using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Actions;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JewishBot.WebHookHandlers.Telegram
{
	public class WebHookHandler
	{
		TelegramBotClient Bot { get; }
		IConfiguration Configuration { get; }

		public WebHookHandler(TelegramBotClient bot, IConfiguration configuration)
		{
			Bot = bot;
			Configuration = configuration;
		}

		public async Task OnMessageRecieved(Message message)
		{
			var command = new CommandParser(message.Text).Parse();
			var chatId = message.Chat.Id;
			var username = message.From.Username;

			switch (command.Name)
			{
				case "echo":
					await new Echo(Bot).HandleAsync(chatId, command.Arguments);
					break;
				case "hey":
					await new Hey(Bot).HandleAsync(chatId);
					break;
				case "ex":
					await new CurrencyExchange(Bot).HandleAsync(chatId, command.Arguments);
					break;
				case "ud":
					await new UrbanDictionary(Bot).HandleAsync(chatId, command.Arguments);
					break;
				case "go":
					await new DuckDuckGo(Bot).HandleAsync(chatId, command.Arguments);
					break;
				case "dice":
					await new RollDice(Bot).HandleAsync(chatId, username, command.Arguments);
					break;
				case "poem":
					await new Poem(Bot).HandleAsync(chatId);
					break;
				case "l":
					await new GoogleMaps(Bot, Configuration["googleApiKey"]).HandleAsync(chatId, command.Arguments);
					break;
				case "advice":
					await new Advice(Bot).HandleAsync(chatId, username);
					break;
				case "weekday":
					await new WeekDay(Bot).HandleAsync(chatId);
					break;
				case "timein":
					await new TimeInPlace(Bot, Configuration["googleApiKey"]).HandleAsync(chatId, command.Arguments);
					break;
				case "calc":
					await new Calc(Bot).HandleAsync(chatId, command.Arguments);
					break;
				case "ball":
					await new MagicBall(Bot).HandleAsync(chatId, command.Arguments);
					break;
			}
		}
	}
}
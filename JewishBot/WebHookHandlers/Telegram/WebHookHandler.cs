using System.Collections.Generic;
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
            var username = string.IsNullOrEmpty(message.From.Username)
                                 ? $"{message.From.FirstName} {message.From.LastName}"
                                 : message.From.Username;

            var commands = new Dictionary<string, IAction>()
            {
                { "echo",    new Echo(Bot, chatId, command.Arguments) },
                { "hey",     new Hey(Bot, chatId) },
                { "ex",      new CurrencyExchange(Bot, chatId, command.Arguments) },
                { "ud",      new UrbanDictionary(Bot, chatId, command.Arguments) },
                { "go",      new DuckDuckGo(Bot, chatId, command.Arguments) },
                { "dice",    new RollDice(Bot, chatId, command.Arguments, username) },
                { "poem",    new Poem(Bot, chatId) },
                { "l",       new GoogleMaps(Bot, chatId, command.Arguments, Configuration["googleApiKey"]) },
                { "advice",  new Advice(Bot, chatId, username) },
                { "weekday", new WeekDay(Bot, chatId) },
                { "timein",  new TimeInPlace(Bot, chatId, command.Arguments, Configuration["googleApiKey"]) },
                { "calc",    new Calc(Bot, chatId, command.Arguments) },
                { "ball",    new MagicBall(Bot, chatId, command.Arguments) }
            };

            if (commands.ContainsKey(command.Name))
            {
                await commands[command.Name].HandleAsync();
            }
		}
	}
}
using JewishBot.WebHookHandlers.Telegram.Actions;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JewishBot.WebHookHandlers.Telegram
{
    public class WebHookHandler
    {
        private TelegramBotClient Bot { get; }
        private IConfiguration Configuration { get; }

        public WebHookHandler(TelegramBotClient bot, IConfiguration configuration)
        {
            Bot = bot;
            Configuration = configuration;
        }

        public void OnMessageRecieved(Message message)
        {
            var command = new CommandParser(message.Text).Parse();
            var chatId = message.Chat.Id;
            var username = message.From.Username;

            switch (command.Name)
            {
                case "echo":
                    new Echo(Bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "hey":
                    new Hey(Bot).HandleAsync(chatId);
                    break;
                case "ex":
                    new CurrencyExchange(Bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "ud":
                    new UrbanDictionary(Bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "go":
                    new DuckDuckGo(Bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "dice":
                    new RollDice(Bot).HandleAsync(chatId, username, command.Arguments);
                    break;
                case "poem":
                    new Poem(Bot).HandleAsync(chatId);
                    break;
                case "l":
                    new GoogleMaps(Bot, Configuration["googleApiKey"]).HandleAsync(chatId, command.Arguments);
                    break;
                case "advice":
                    new Advice(Bot).HandleAsync(chatId, username);
                    break;
                case "weekday":
                    new WeekDay(Bot).HandleAsync(chatId);
                    break;
                case "timein":
                    new TimeInPlace(Bot, Configuration["googleApiKey"]).HandleAsync(chatId, command.Arguments);
                    break;
            }
        }
    }
}
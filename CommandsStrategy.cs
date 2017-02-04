using JewishBot.Actions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JewishBot
{
    public class CommandsHandler
    {
        private TelegramBotClient Bot { get; }
        private Message InputMessage { get; }

        public CommandsHandler(TelegramBotClient bot, Message message)
        {
            Bot = bot;
            InputMessage = message;
        }

        public void Execute(Command command)
        {
            var chatId = InputMessage.Chat.Id;
            var username = InputMessage.From.Username;

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
                    new GoogleMaps(Bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "advice":
                    new Advice(Bot).HandleAsync(chatId, username);
                    break;
                case "weekday":
                    new WeekDay(Bot).HandleAsync(chatId);
                    break;
            }
        }
    }
}
using JewishBot.Actions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JewishBot
{
    public class CommandsHandler
    {
        private readonly TelegramBotClient _bot;
        private readonly Message _message;

        public CommandsHandler(TelegramBotClient bot, Message message)
        {
            _bot = bot;
            _message = message;
        }

        public void Execute(Command command)
        {
            var chatId = _message.Chat.Id;
            var username = _message.From.Username;

            switch (command.Name)
            {
                case "echo":
                    new Echo(_bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "hey":
                    new Hey(_bot).HandleAsync(chatId);
                    break;
                case "ex":
                    new CurrencyExchange(_bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "ud":
                    new UrbanDictionary(_bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "go":
                    new DuckDuckGo(_bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "dice":
                    new RollDice(_bot).HandleAsync(chatId, username, command.Arguments);
                    break;
                case "poem":
                    new Poem(_bot).HandleAsync(chatId);
                    break;
            }
        }
    }
}
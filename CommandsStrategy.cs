using JewishBot.Actions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JewishBot {
    public class CommandsHandler {

        private readonly TelegramBotClient bot;
        private readonly Message message;

        public CommandsHandler(TelegramBotClient bot, Message message) {
            this.bot = bot;
            this.message = message;
        }
        public void Execute(Command command) {
            long chatId = message.Chat.Id;
            string username = message.From.Username;

            switch (command.Name) {
                case "echo":
                    new Echo(bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "hey":
                    new Hey(bot).HandleAsync(chatId);
                    break;
                case "ex":
                    new CurrencyExchange(bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "ud":
                    new UrbanDictionary(bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "go":
                    new DuckDuckGo(bot).HandleAsync(chatId, command.Arguments);
                    break;
                case "dice":
                    new RollDice(bot).HandleAsync(chatId, username, command.Arguments);
                    break;
            }
        }
    }
}
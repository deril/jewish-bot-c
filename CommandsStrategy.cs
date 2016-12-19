using JewishBot.Actions;
using Telegram.Bot;

namespace JewishBot {
    public class CommandsHandler {

        private readonly TelegramBotClient bot;

        public CommandsHandler(TelegramBotClient bot) {
            this.bot = bot;
        }
        public void Execute(Command command, long chatId) {
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
            }
        }
    }
}
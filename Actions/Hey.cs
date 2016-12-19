using Telegram.Bot;

namespace JewishBot.Actions {
    public class Hey : IAction {

        private readonly TelegramBotClient bot;
        public Hey(TelegramBotClient bot) {
            this.bot = bot;
        }
        public static string Description { get; } = @"Helloes to sender. 
            Usage: /hey";

        public async void HandleAsync(long chatId, string[] args = null) {
            await bot.SendTextMessageAsync(chatId, "Hey!");
        }
    }
}
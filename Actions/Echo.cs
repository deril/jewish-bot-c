using System;
using Telegram.Bot;

namespace JewishBot.Actions {
    public class Echo : IAction {

        private readonly TelegramBotClient bot;
        public Echo(TelegramBotClient bot) {
            this.bot = bot;
        }
        public static string Description { get; } = @"Returns input text. 
            Usage: /echo <text>";

        public async void HandleAsync(long chatId, string[] args) {
            if (args == null) return;
            await bot.SendTextMessageAsync(chatId, String.Join(" ", args));
        }
    }
}
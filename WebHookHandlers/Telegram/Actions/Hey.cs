using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    internal class Hey : IAction
    {
        private TelegramBotClient Bot { get; }

        public Hey(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public static string Description { get; } = @"Helloes to sender.
            Usage: /hey";

        public async void HandleAsync(long chatId, string[] args = null)
        {
            await Bot.SendTextMessageAsync(chatId, "Hey!");
        }
    }
}
using Telegram.Bot;

namespace JewishBot.Actions
{
    internal class Hey : IAction
    {
        private readonly TelegramBotClient _bot;

        public Hey(TelegramBotClient bot)
        {
            _bot = bot;
        }

        public static string Description { get; } = @"Helloes to sender.
            Usage: /hey";

        public async void HandleAsync(long chatId, string[] args = null)
        {
            await _bot.SendTextMessageAsync(chatId, "Hey!");
        }
    }
}
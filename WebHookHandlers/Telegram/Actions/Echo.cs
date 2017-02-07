using Telegram.Bot;

namespace JewishBot.WebHookHanders.Telegram.Actions
{
    internal class Echo : IAction
    {
        private TelegramBotClient Bot { get; }

        public Echo(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public static string Description { get; } = @"Returns input text.
            Usage: /echo <text>";

        public async void HandleAsync(long chatId, string[] args)
        {
            if (args == null) return;
            await Bot.SendTextMessageAsync(chatId, string.Join(" ", args));
        }
    }
}

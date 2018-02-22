namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Threading.Tasks;
    using global::Telegram.Bot;

    internal class Hey : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;

        public Hey(TelegramBotClient bot, long chatId)
        {
            this.bot = bot;
            this.chatId = chatId;
        }

        public static string Description { get; } = @"Helloes to sender.
            Usage: /hey";

        public async Task HandleAsync()
        {
            await this.bot.SendTextMessageAsync(this.chatId, "היי! (Hey)");
        }
    }
}
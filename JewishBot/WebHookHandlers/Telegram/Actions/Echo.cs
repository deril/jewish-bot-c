namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Telegram.Bot;

    internal class Echo : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private IReadOnlyCollection<string> args;

        public Echo(TelegramBotClient bot, long chatId, IReadOnlyCollection<string> args)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
        }

        public static string Description { get; } = @"Returns input text.
            Usage: /echo <text>";

        public async Task HandleAsync()
        {
            if (this.args == null)
            {
                return;
            }

            await this.bot.SendTextMessageAsync(this.chatId, string.Join(" ", this.args));
        }
    }
}

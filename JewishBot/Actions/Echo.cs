namespace JewishBot.Actions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class Echo : IAction
    {
        private readonly IReadOnlyCollection<string> args;
        private readonly IBotService botService;
        private readonly long chatId;

        public Echo(IBotService botService, long chatId, IReadOnlyCollection<string> args)
        {
            this.botService = botService;
            this.chatId = chatId;
            this.args = args;
        }

        public async Task HandleAsync()
        {
            if (this.args == null)
            {
                return;
            }

            await this.botService.Client.SendTextMessageAsync(this.chatId, string.Join(" ", this.args));
        }
    }
}
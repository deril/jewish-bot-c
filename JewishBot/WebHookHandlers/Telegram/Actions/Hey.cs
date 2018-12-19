namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Threading.Tasks;

    internal class Hey : IAction
    {
        private readonly IBotService botService;
        private readonly long chatId;

        public Hey(IBotService botService, long chatId)
        {
            this.botService = botService;
            this.chatId = chatId;
        }

        public async Task HandleAsync()
        {
            await this.botService.Client.SendTextMessageAsync(this.chatId, "היי! (Hey)");
        }
    }
}
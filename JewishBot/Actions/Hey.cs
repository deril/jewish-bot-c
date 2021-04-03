namespace JewishBot.Actions
{
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class Hey : IAction
    {
        private readonly IBotService _botService;
        private readonly long _chatId;

        public Hey(IBotService botService, long chatId)
        {
            _botService = botService;
            _chatId = chatId;
        }

        public async Task HandleAsync()
        {
            await _botService.Client.SendTextMessageAsync(_chatId, "היי! (Hey)");
        }
    }
}
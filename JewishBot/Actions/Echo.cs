namespace JewishBot.Actions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class Echo : IAction
    {
        private readonly IReadOnlyCollection<string> _args;
        private readonly IBotService _botService;
        private readonly long _chatId;

        public Echo(IBotService botService, long chatId, IReadOnlyCollection<string> args)
        {
            _botService = botService;
            _chatId = chatId;
            _args = args;
        }

        public async Task HandleAsync()
        {
            if (_args == null)
            {
                return;
            }

            await _botService.Client.SendTextMessageAsync(_chatId, string.Join(" ", _args));
        }
    }
}
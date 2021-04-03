namespace JewishBot.Actions.UrbanDictionary
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class UrbanDictionary : IAction
    {
        private readonly IReadOnlyCollection<string> _args;
        private readonly IBotService _botService;
        private readonly long _chatId;
        private readonly IHttpClientFactory _clientFactory;

        public UrbanDictionary(IBotService botService, IHttpClientFactory clientFactory, long chatId,
            IReadOnlyCollection<string> args)
        {
            _botService = botService;
            _clientFactory = clientFactory;
            _chatId = chatId;
            _args = args;
        }

        public async Task HandleAsync()
        {
            var message = "Please specify at least 1 search term";

            if (_args != null)
            {
                var ud = new DictApi(_clientFactory);
                var result = await ud.InvokeAsync(_args);
                message = result.Errors == null && result.List.Count > 0 ? result.List[0].Definition : result.Errors;
            }

            await _botService.Client.SendTextMessageAsync(_chatId,
                string.IsNullOrEmpty(message) ? "Nothing found \uD83D\uDE22" : message);
        }
    }
}
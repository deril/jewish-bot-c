namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JewishBot.Attributes;
    using Services.UrbanDictionary;

    internal class UrbanDictionary : IAction
    {
        private readonly IBotService botService;
        private readonly long chatId;
        private readonly IHttpClientFactory clientFactory;
        private readonly IReadOnlyCollection<string> args;

        public UrbanDictionary(IBotService botService, IHttpClientFactory clientFactory, long chatId, IReadOnlyCollection<string> args)
        {
            this.botService = botService;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
            this.args = args;
        }

        [RequestRateLimit(Name = "Limit Request Number", Seconds = 10)]
        public async Task HandleAsync()
        {
            var message = "Please specify at least 1 search term";

            if (this.args != null)
            {
                var ud = new DictApi(this.clientFactory);
                var result = await ud.InvokeAsync(this.args);
                message = result.Errors == null && result.List.Count > 0 ? result.List[0].Definition : result.Errors;
            }

            await this.botService.Client.SendTextMessageAsync(this.chatId, string.IsNullOrEmpty(message) ? "Nothing found \uD83D\uDE22" : message);
        }
    }
}
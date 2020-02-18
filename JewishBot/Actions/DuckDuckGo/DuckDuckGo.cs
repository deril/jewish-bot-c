namespace JewishBot.Actions.DuckDuckGo
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class DuckDuckGo : IAction
    {
        private readonly IBotService botService;
        private readonly long chatId;
        private readonly IHttpClientFactory clientFactory;
        private readonly IReadOnlyCollection<string> args;

        public DuckDuckGo(IBotService botService, IHttpClientFactory clientFactory, long chatId, IReadOnlyCollection<string> args)
        {
            this.botService = botService;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
            this.args = args;
        }

        public async Task HandleAsync()
        {
            var message = "Please specify at least 1 search term";
            if (this.args != null)
            {
                var go = new GoApi(this.clientFactory);
                var result = await go.InvokeAsync(this.args);
                message = result.Type switch
                {
                    "A" => result.AbstractText,
                    "D" => result.RelatedTopics[0].Text,
                    "E" => result.Redirect,
                    "C" => result.AbstractUrl.ToString(),
                    _ => "Nothing found \uD83D\uDE22"
                };
            }

            await this.botService.Client.SendTextMessageAsync(this.chatId, message ?? "Nothing found \uD83D\uDE22");
        }
    }
}

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Services.DuckDuckGo;

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
                switch (result.Type)
                {
                    case "A":
                        message = result.AbstractText;
                        break;
                    case "D":
                        message = result.RelatedTopics[0].Text;
                        break;
                    case "E":
                        message = result.Redirect;
                        break;
                    case "C":
                        message = result.AbstractUrl.ToString();
                        break;
                    default:
                        message = "Nothing found \uD83D\uDE22";
                        break;
                }
            }

            await this.botService.Client.SendTextMessageAsync(this.chatId, message);
        }
    }
}

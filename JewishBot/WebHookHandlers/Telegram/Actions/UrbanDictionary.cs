namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Services.UrbanDictionary;

    internal class UrbanDictionary : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly IHttpClientFactory clientFactory;
        private IReadOnlyCollection<string> args;

        public UrbanDictionary(TelegramBotClient bot, IHttpClientFactory clientFactory, long chatId, IReadOnlyCollection<string> args)
        {
            this.bot = bot;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
            this.args = args;
        }

        public async Task HandleAsync()
        {
            var message = "Please specify at least 1 search term";

            if (this.args != null)
            {
                var ud = new DictApi(this.clientFactory);
                var result = await ud.InvokeAsync(this.args);
                message = result.Errors == null && result.List.Count > 0 ? result.List[0].Definition : "Nothing found \uD83D\uDE22";
            }

            await this.bot.SendTextMessageAsync(this.chatId, message);
        }
    }
}
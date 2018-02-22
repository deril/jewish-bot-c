namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Services.UrbanDictionary;

    internal class UrbanDictionary : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly string[] args;

        public UrbanDictionary(TelegramBotClient bot, long chatId, string[] args)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
        }

        public async Task HandleAsync()
        {
            var message = "Please specify at least 1 search term";

            if (this.args != null)
            {
                var ud = new DictApi();
                var result = await ud.InvokeAsync<QueryModel>(new string[] { string.Join(" ", this.args) });
                message = result.ResultType == "exact" ? result.List[0].Definition : "Nothing found \uD83D\uDE22";
            }

            await this.bot.SendTextMessageAsync(this.chatId, message);
        }
    }
}
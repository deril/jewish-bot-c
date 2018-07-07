namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Globalization;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types.Enums;
    using JewishBot.WebHookHandlers.Services.Poem;

    internal class Poem : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly IHttpClientFactory clientFactory;

        public Poem(TelegramBotClient bot, IHttpClientFactory clientFactory, long chatId)
        {
            this.bot = bot;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
        }

        public async Task HandleAsync()
        {
            var poemApi = new PoemApi(this.clientFactory);
            var result = await poemApi.InvokeAsync();
            if (result.Error != null)
            {
                await this.bot.SendTextMessageAsync(this.chatId, result.Error, parseMode: ParseMode.Markdown);
                return;
            }

            var culture = new CultureInfo("uk-UA", true);
            var str = new StringBuilder();
            str.AppendFormat(culture, "*{0}*", result.Title);
            str.Append("\n\n");
            str.Append(string.Join("\n", result.Lines));

            await this.bot.SendTextMessageAsync(this.chatId, str.ToString());
        }
    }
}
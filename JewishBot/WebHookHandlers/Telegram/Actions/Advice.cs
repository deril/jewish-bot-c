namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Services.GreatAdvice;

    public class Advice : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly string username;
        private readonly IHttpClientFactory clientFactory;

        public Advice(TelegramBotClient bot, IHttpClientFactory clientFactory, long chatId, string username)
        {
            this.bot = bot;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
            this.username = username;
        }

        public async Task HandleAsync()
        {
            var adviceService = new GreatAdviceApi(this.clientFactory);
            var result = await adviceService.InvokeAsync();
            var textResult = WebUtility.HtmlDecode(result.Text);
            var message = $"Advice for {this.username}: {textResult}";

            await this.bot.SendTextMessageAsync(this.chatId, message);
        }
    }
}
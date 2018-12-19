namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Services.GreatAdvice;

    public class Advice : IAction
    {
        private readonly IBotService botService;
        private readonly long chatId;
        private readonly string username;
        private readonly IHttpClientFactory clientFactory;

        public Advice(IBotService botService, IHttpClientFactory clientFactory, long chatId, string username)
        {
            this.botService = botService;
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

            await this.botService.Client.SendTextMessageAsync(this.chatId, message);
        }
    }
}
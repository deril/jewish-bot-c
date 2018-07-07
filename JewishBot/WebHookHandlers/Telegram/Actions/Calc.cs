namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Services.Mathjs;

    public class Calc : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly IHttpClientFactory clientFactory;
        private IReadOnlyCollection<string> args;

        public Calc(TelegramBotClient bot, IHttpClientFactory clientFactory, long chatId, IReadOnlyCollection<string> args)
        {
            this.bot = bot;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
            this.args = args;
        }

        public static string Description { get; } = @"Calculate using math.js API.
Usage: /calc <query>";

        public async Task HandleAsync()
        {
            var message = await this.PrepareMessageAsync();
            await this.bot.SendTextMessageAsync(this.chatId, message);
        }

        private async Task<string> PrepareMessageAsync()
        {
            if (this.args == null)
            {
                return Description;
            }

            try
            {
                var mathjsApi = new MathjsApi(this.clientFactory);
                var answer = await mathjsApi.InvokeAsync(this.args);
                var question = string.Join(" ", this.args);
                return answer.Error ? $"Unable to evaluate {question}" : $"{question} => {answer.Answer}";
            }
            catch
            {
                return "Unable to reach evaluation server.";
            }
        }
    }
}
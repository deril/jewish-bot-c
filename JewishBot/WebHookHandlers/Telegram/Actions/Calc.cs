namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Services.Mathjs;

    public class Calc : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly string[] args;

        public Calc(TelegramBotClient bot, long chatId, string[] args)
        {
            this.bot = bot;
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

            var question = string.Join(" ", this.args);
            var mathjsApi = new MathjsApi();

            try
            {
                var answer = await mathjsApi.InvokeAsync<QueryModel>(new string[] { question });
                return answer.Error ? $"Unable to evaluate {question}" : $"{question}, {answer.Answer}";
            }
            catch
            {
                return "Unable to reach evaluation server.";
            }
        }
    }
}
namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Microsoft.Extensions.Configuration;
    using Services.Lunch;

    internal class Lunch : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly string[] args;
        private readonly IConfiguration config;

        public Lunch(TelegramBotClient bot, long chatId, string[] args, IConfiguration config)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
            this.config = config;
        }

        public async Task HandleAsync()
        {
            var members = this.args == null ? this.config["lunch:members"] : string.Join(string.Empty, this.args);
            var lunchApi = new LunchApi(this.config["lunch:email"], this.config["lunch:password"], members);
            await this.bot.SendTextMessageAsync(this.chatId, lunchApi.Invoke());
        }
    }
}

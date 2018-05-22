namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Linq;
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using JewishBot.Data;
    using Microsoft.Extensions.Configuration;
    using Services.Lunch;

    internal class Lunch : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly string[] args;
        private readonly IConfiguration config;
        private readonly IUserRepository repository;

        public Lunch(TelegramBotClient bot, long chatId, string[] args, IConfiguration config, IUserRepository repo)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
            this.config = config;
            this.repository = repo;
        }

        public async Task HandleAsync()
        {
            var members = this.args == null ? this.repository.Users.Select(user => user.LunchName).ToArray() : this.args;
            var lunchApi = new LunchApi(this.config["lunch:email"], this.config["lunch:password"], members);
            await this.bot.SendTextMessageAsync(this.chatId, lunchApi.Invoke());
        }
    }
}

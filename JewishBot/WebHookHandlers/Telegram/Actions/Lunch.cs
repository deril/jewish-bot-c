namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Configuration;
    using Services.Lunch;

    internal class Lunch : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly int userId;
        private readonly ChatType chatType;
        private readonly string[] args;
        private readonly IConfiguration config;
        private readonly IUserRepository repository;

        public Lunch(TelegramBotClient bot, long chatId, int userId, ChatType chatType, string[] args, IConfiguration config, IUserRepository repo)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.userId = userId;
            this.chatType = chatType;
            this.args = args;
            this.config = config;
            this.repository = repo;
        }

        public async Task HandleAsync()
        {
            string[] members;

            // if user send something via arguments use that values, otherwise look for its name in repo
            // if nothing found or group message use predefined names
            if (this.args == null && this.chatType == ChatType.Private)
            {
                var member = this.repository.Users.FirstOrDefault(u => u.TelegramID == this.userId)?.LunchName;
                members = member == null ? this.config["lunch:members"].Split(",") : new[] { member };
            }
            else
            {
                members = this.args ?? this.config["lunch:members"].Split(",");
            }

            var lunchApi = new LunchApi(this.config["lunch:email"], this.config["lunch:password"], members);
            await this.bot.SendTextMessageAsync(this.chatId, lunchApi.Invoke());
        }
    }
}

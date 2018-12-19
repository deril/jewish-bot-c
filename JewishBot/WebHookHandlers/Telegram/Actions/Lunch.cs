namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Data;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types.Enums;
    using Microsoft.Extensions.Configuration;
    using Services.Lunch;

    internal class Lunch : IAction
    {
        private readonly IBotService botService;
        private readonly long chatId;
        private readonly int userId;
        private readonly ChatType chatType;
        private readonly IReadOnlyCollection<string> args;
        private readonly IConfiguration config;
        private readonly IUserRepository repository;
        private readonly IHttpClientFactory clientFactory;

        public Lunch(IBotService botService, long chatId, int userId, ChatType chatType, IReadOnlyCollection<string> args, IConfiguration config, IUserRepository repo, IHttpClientFactory clientFactory)
        {
            this.botService = botService;
            this.chatId = chatId;
            this.userId = userId;
            this.chatType = chatType;
            this.args = args;
            this.config = config;
            this.repository = repo;
            this.clientFactory = clientFactory;
        }

        public async Task HandleAsync()
        {
            string[] members;

            // if user send something via arguments use that values, otherwise look for its name in repo
            // if nothing found or group message use predefined names
            if (this.CanUsePresetName())
            {
                var member = this.repository.Users.FirstOrDefault(u => u.TelegramId == this.userId)?.LunchName;
                members = member == null ? this.MembersFromConfig() : new[] { member };
            }
            else
            {
                members = this.args.Count == 0 ? this.MembersFromConfig() : this.args.ToArray();
            }

            var lunchApi = new LunchApi(this.config["lunch:email"], this.config["lunch:password"], members, this.clientFactory);
            await this.botService.Client.SendTextMessageAsync(this.chatId, lunchApi.Invoke());
        }

        private string[] MembersFromConfig()
        {
            return this.config["lunch:members"].Split(",");
        }

        private bool CanUsePresetName()
        {
            return this.IsNoArgumentsForAction() && this.IsPrivateChat();
        }

        private bool IsNoArgumentsForAction()
        {
            return this.args == null;
        }

        private bool IsPrivateChat()
        {
            return this.chatType == ChatType.Private;
        }
    }
}

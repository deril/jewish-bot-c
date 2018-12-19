namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using global::Telegram.Bot.Types.Enums;
    using Models;

    internal class SetLunch : IAction
    {
        private readonly IBotService botService;
        private readonly long chatId;
        private readonly int userId;
        private readonly ChatType chatType;
        private readonly IUserRepository repository;
        private readonly IReadOnlyCollection<string> args;

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "This is OK here.")]
        public SetLunch(IBotService botService, long chatId, int userId, ChatType chatType, IReadOnlyCollection<string> args, IUserRepository repo)
        {
            (this.botService, this.chatId, this.userId, this.chatType, this.args, this.repository) = (botService, chatId, userId, chatType, args, repo);
        }

        public async Task HandleAsync()
        {
            if (this.chatType != ChatType.Private)
            {
                const string message = "Allowed only in private conversation!";
                await this.botService.Client.SendTextMessageAsync(this.chatId, message).ConfigureAwait(false);
                return;
            }

            var user = this.repository.Users.FirstOrDefault(u => u.TelegramId == this.userId) ?? new User()
            {
                UserId = 0,
                TelegramId = this.userId,
            };

            user.LunchName = string.Join(" ", this.args);

            this.repository.SaveUser(user);

            await this.botService.Client.SendTextMessageAsync(this.chatId, $"Your lunch name successfully set to {user.LunchName}");
        }
    }
}

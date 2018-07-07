namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types.Enums;
    using JewishBot.Data;
    using JewishBot.Models;

    internal class SetLunch : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly int userId;
        private readonly ChatType chatType;
        private readonly IUserRepository repository;
        private IReadOnlyCollection<string> args;

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "This is OK here.")]
        public SetLunch(TelegramBotClient bot, long chatId, int userId, ChatType chatType, IReadOnlyCollection<string> args, IUserRepository repo)
        {
            (this.bot, this.chatId, this.userId, this.chatType, this.args, this.repository) = (bot, chatId, userId, chatType, args, repo);
        }

        public async Task HandleAsync()
        {
            string message;
            if (this.chatType != ChatType.Private)
            {
                message = "Allowed only in private conversation!";
                await this.bot.SendTextMessageAsync(this.chatId, message).ConfigureAwait(false);
                return;
            }

            var user = this.repository.Users.FirstOrDefault(u => u.TelegramID == this.userId);
            if (user == null)
            {
                user = new User()
                {
                    UserID = 0,
                    TelegramID = this.userId,
                };
            }

            user.LunchName = string.Join(" ", this.args);

            this.repository.SaveUser(user);

            await this.bot.SendTextMessageAsync(this.chatId, $"Your lunch name successfuly set to {user.LunchName}");
        }
    }
}

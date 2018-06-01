namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Text;
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Args;
    using global::Telegram.Bot.Types.Enums;
    using global::Telegram.Bot.Types.ReplyMarkups;
    using JewishBot.WebHookHandlers.Telegram.Services.Poem;

    internal class Poem : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly PoemApi poemApi;

        public Poem(TelegramBotClient bot, long chatId)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.poemApi = new PoemApi();
        }

        public async Task HandleAsync()
        {
            var result = await this.poemApi.InvokeAsync<QueryModel>(null);
            if (result.Error != null)
            {
                await this.bot.SendTextMessageAsync(this.chatId, result.Error, parseMode: ParseMode.Markdown);
                return;
            }

            var str = new StringBuilder();
            str.AppendFormat("*{0}*", result.Title);
            str.Append("\n\n");
            str.Append(string.Join("\n", result.Lines));

            var keyboard =
                new InlineKeyboardMarkup(new[] { InlineKeyboardButton.WithCallbackData("\uD83D\uDC4D Like", result.Hashid) });

            this.bot.OnCallbackQuery += this.OnLikedPoemAsync;

            await this.bot.SendTextMessageAsync(this.chatId, str.ToString(), parseMode: ParseMode.Markdown, replyMarkup: keyboard);
        }

        private async void OnLikedPoemAsync(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await this.poemApi.Like(callbackQueryEventArgs.CallbackQuery.Data);

            await this.bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id, "Thanks for your like!");
        }
    }
}
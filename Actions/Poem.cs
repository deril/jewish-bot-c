using System.Text;
using JewishBot.Services.Poem;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace JewishBot.Actions
{
    internal class Poem : IAction
    {
        private readonly TelegramBotClient _bot;
        private readonly PoemApi _poemService = new PoemApi();

        public Poem(TelegramBotClient bot)
        {
            _bot = bot;
        }

        public async void HandleAsync(long chatId)
        {
            var result = await _poemService.Invoke<QueryModel>(null);
            if (result.Error != null)
            {
                await _bot.SendTextMessageAsync(chatId, result.Error, parseMode: ParseMode.Markdown);
                return;
            }

            var str = new StringBuilder();
            str.AppendFormat("*{0}*", result.Title);
            str.Append("\n\n");
            str.Append(string.Join("\n", result.Lines));

            var keyboard =
                new InlineKeyboardMarkup(new[] {new InlineKeyboardButton("\uD83D\uDC4D Like", result.Hashid)});

            _bot.OnCallbackQuery += OnLikedPoemAsync;

            await _bot.SendTextMessageAsync(chatId, str.ToString(), parseMode: ParseMode.Markdown,
                replyMarkup: keyboard);
        }

        private async void OnLikedPoemAsync(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            _poemService.Like(callbackQueryEventArgs.CallbackQuery.Data);

            await _bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                "Thanks for your like!", cacheTime: 1);
        }
    }
}
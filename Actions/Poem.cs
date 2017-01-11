using System;
using System.Text;
using JewishBot.Services.Poem;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace JewishBot.Actions {
    class Poem : IAction {
        private readonly TelegramBotClient bot;
        private readonly PoemApi poemService = new PoemApi();
        public Poem(TelegramBotClient bot) {
            this.bot = bot;
        }

        public async void HandleAsync(long chatId) {
            var result = await poemService.Invoke<QueryModel>(null);
            if (result.Error != null) {
                await bot.SendTextMessageAsync(chatId, result.Error, parseMode: ParseMode.Markdown);
                return;
            }

            var str = new StringBuilder();
            str.AppendFormat("*{0}*", result.Title);
            str.Append("\n\n");
            str.Append(String.Join("\n", result.Lines));

            var keyboard = new InlineKeyboardMarkup(new [] {new InlineKeyboardButton("\uD83D\uDC4D Like", result.Hashid)});

            bot.OnCallbackQuery += OnLikedPoemAsync;
            
            await bot.SendTextMessageAsync(chatId, str.ToString(), parseMode: ParseMode.Markdown, replyMarkup: keyboard);
        }

        private async void OnLikedPoemAsync(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            poemService.Like(callbackQueryEventArgs.CallbackQuery.Data);

            await bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                "Thanks for your like!", cacheTime: 1);
        }
    }
}
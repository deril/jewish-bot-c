//using System.Text;
//using System.Threading.Tasks;
//using JewishBot.WebHookHandlers.Telegram.Services.Poem;
//using Telegram.Bot;
//using Telegram.Bot.Args;
//using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.InlineKeyboardButtons;
//using Telegram.Bot.Types.ReplyMarkups;

//namespace JewishBot.WebHookHandlers.Telegram.Actions
//{
//    class Poem : IAction
//    {
//        TelegramBotClient Bot { get; }
//        long ChatId { get; }
//        PoemApi PoemService { get; } = new PoemApi();

//        public Poem(TelegramBotClient bot, long chatId)
//        {
//            Bot = bot;
//            ChatId = chatId;
//        }

//        public async Task HandleAsync()
//        {
//            var result = await PoemService.InvokeAsync<QueryModel>(null);
//            if (result.Error != null)
//            {
//                await Bot.SendTextMessageAsync(ChatId, result.Error, parseMode: ParseMode.Markdown);
//                return;
//            }

//            var str = new StringBuilder();
//            str.AppendFormat("*{0}*", result.Title);
//            str.Append("\n\n");
//            str.Append(string.Join("\n", result.Lines));

//            var keyboard =
//                new InlineKeyboardMarkup(new[] { new InlineKeyboardCallbackButton("\uD83D\uDC4D Like", result.Hashid) });

//            Bot.OnCallbackQuery += OnLikedPoemAsync;

//            await Bot.SendTextMessageAsync(ChatId, str.ToString(), parseMode: ParseMode.Markdown,
//                replyMarkup: keyboard);
//        }

//        async void OnLikedPoemAsync(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
//        {
//            await PoemService.Like(callbackQueryEventArgs.CallbackQuery.Data);

//            await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
//                "Thanks for your like!");
//        }
//    }
//}
using System.Text;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.Poem;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	class Poem : IAction
	{
		TelegramBotClient Bot { get; }
		PoemApi PoemService { get; } = new PoemApi();

		public Poem(TelegramBotClient bot)
		{
			Bot = bot;
		}

		public async Task HandleAsync(long chatId)
		{
			var result = await PoemService.Invoke<QueryModel>(null);
			if (result.Error != null)
			{
				await Bot.SendTextMessageAsync(chatId, result.Error, parseMode: ParseMode.Markdown);
				return;
			}

			var str = new StringBuilder();
			str.AppendFormat("*{0}*", result.Title);
			str.Append("\n\n");
			str.Append(string.Join("\n", result.Lines));

			var keyboard =
				new InlineKeyboardMarkup(new[] { new InlineKeyboardButton("\uD83D\uDC4D Like", result.Hashid) });

			Bot.OnCallbackQuery += OnLikedPoemAsync;

			await Bot.SendTextMessageAsync(chatId, str.ToString(), parseMode: ParseMode.Markdown,
				replyMarkup: keyboard);
		}

		async void OnLikedPoemAsync(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
		{
			await PoemService.Like(callbackQueryEventArgs.CallbackQuery.Data);

			await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
				"Thanks for your like!");
		}
	}
}
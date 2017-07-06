using System.Collections.Generic;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.Mathjs;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	public class Calc : IAction
	{
		TelegramBotClient Bot { get; }
		long ChatId { get; }
		string[] Args { get; }

		public static string Description { get; } = @"Calculate using math.js API.
Usage: /calc <query>";

		public Calc(TelegramBotClient bot, long chatId, string[] args)
		{
			Bot = bot;
			ChatId = chatId;
			Args = args;
		}

		public async Task HandleAsync()
		{
			var message = await PrepareMessageAsync();
			await Bot.SendTextMessageAsync(ChatId, message);
		}

		async Task<string> PrepareMessageAsync()
		{
			if (Args == null) return Description;

			var question = string.Join(" ", Args);
			var mathjsApi = new MathjsApi();

			try
			{
				var answer = await mathjsApi.Invoke<QueryModel>(question);
				return answer.Error ? $"Unable to evaluate {question}" : $"{question}, {answer.Answer}";
			}
			catch
			{
				return "Unable to reach evaluation server.";
			}
		}
	}
}
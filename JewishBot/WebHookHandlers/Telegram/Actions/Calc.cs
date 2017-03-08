using System.Collections.Generic;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.Mathjs;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    public class Calc : IAction
    {
		TelegramBotClient Bot { get; }

		public static string Description { get; } = @"Calculate using math.js API.
Usage: /calc <query>";

        public Calc(TelegramBotClient bot)
        {
            Bot = bot;
        }

		public async Task HandleAsync(long chatId, string[] args)
        {
            var message = await PrepareMessageAsync(args);
            await Bot.SendTextMessageAsync(chatId, message);
        }

		static async Task<string> PrepareMessageAsync(IReadOnlyList<string> args)
		{
			if (args == null) return Description;
			var question = string.Join(" ", args);
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
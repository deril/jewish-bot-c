using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
	public class MagicBall : IAction
	{
		TelegramBotClient Bot { get; }
		long ChatId { get; }
		string[] Args { get; }

		Random Rnd { get; } = new Random();

		public static string Description { get; } = @"Predicts a future.
			Usage: /ball <question>";

		List<string> Answers { get; } = new List<string>
		{
			"It is certain",
			"It is decidedly so",
			"Without a doubt",
			"Yes definitely",
			"You may rely on it",
			"As I see it, yes",
			"Most likely",
			"Outlook good",
			"Yes",
			"Signs point to yes",
			"Don't count on it",
			"My reply is no",
			"My sources say no",
			"Outlook not so good",
			"Very doubtful"
		};

		List<string> AskAgainAnswers { get; } = new List<string>
		{
			"Reply hazy try again",
			"Ask again later",
			"Better not tell you now",
			"Cannot predict now",
			"Concentrate and ask again"
		};

		public MagicBall(TelegramBotClient bot, long chatId, string[] args)
		{
			Bot = bot;
			ChatId = chatId;
			Args = args;
		}

		public async Task HandleAsync()
		{
			if (Args == null)
			{
				await Bot.SendTextMessageAsync(ChatId, Description);
				return;
			}

			if (Rnd.Next(1, 6) == 1)
			{
				var index = Rnd.Next(AskAgainAnswers.Count + 1);
				await Bot.SendTextMessageAsync(ChatId, AskAgainAnswers[index]);
			}
			else
			{
				using (var algorithm = SHA256.Create())
				{
					var time = DateTime.Now.Ticks;
					var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(string.Join(" ", Args) + time));
					var index = (int)(ConvertHash(hash) % Answers.Count);
					await Bot.SendTextMessageAsync(ChatId, Answers[index]);
				}
			}
		}

		BigInteger ConvertHash(byte[] hash)
		{
			// make int unsigned
			var uhash = new byte[hash.Length + 1];
			hash.CopyTo(uhash, 0);
			uhash[hash.Length] = 00;
			return new BigInteger(uhash);
		}
	}
}

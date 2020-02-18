namespace JewishBot.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class MagicBall : IAction
    {
        private const string Description = @"Predicts a future.
            Usage: /ball <question>";

        private readonly List<string> answers = new List<string>
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

        private readonly IReadOnlyCollection<string> args;

        private readonly List<string> askAgainAnswers = new List<string>
        {
            "Reply hazy try again",
            "Ask again later",
            "Better not tell you now",
            "Cannot predict now",
            "Concentrate and ask again"
        };

        private readonly IBotService botService;
        private readonly long chatId;
        private readonly Random rnd = new Random();

        public MagicBall(IBotService botService, long chatId, IReadOnlyCollection<string> args)
        {
            this.botService = botService;
            this.chatId = chatId;
            this.args = args;
        }

        public async Task HandleAsync()
        {
            if (this.args == null)
            {
                await this.botService.Client.SendTextMessageAsync(this.chatId, Description).ConfigureAwait(false);
                return;
            }

            if (this.rnd.Next(1, 6) == 1)
            {
                var index = this.rnd.Next(this.askAgainAnswers.Count + 1);
                await this.botService.Client.SendTextMessageAsync(this.chatId, this.askAgainAnswers[index])
                    .ConfigureAwait(false);
            }
            else
            {
                using var algorithm = SHA256.Create();
                var time = DateTime.Now.Ticks;
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(string.Join(" ", this.args) + time));
                var index = (int) (ConvertHash(hash) % this.answers.Count);
                await this.botService.Client.SendTextMessageAsync(this.chatId, this.answers[index])
                    .ConfigureAwait(false);
            }
        }

        private static BigInteger ConvertHash(byte[] hash)
        {
            // make int unsigned
            var uhash = new byte[hash.Length + 1];
            hash.CopyTo(uhash, 0);
            uhash[hash.Length] = 00;
            return new BigInteger(uhash);
        }
    }
}
namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Services.DiceGame;

    internal class RollDice : IAction
    {
        private const string DefaultPatern = "1d6";
        private readonly long chatId;
        private readonly string username;
        private readonly TelegramBotClient bot;
        private ReadOnlyCollection<string> args;

        public RollDice(TelegramBotClient bot, long chatId, ReadOnlyCollection<string> args, string username)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
            this.username = username;
        }

        public async Task HandleAsync()
        {
            var toParse = this.IsParsableArguments() ? this.args[0] : DefaultPatern;
            var result = new Dice(toParse);

            var message = result.GetSum();

            await this.bot.SendTextMessageAsync(this.chatId, $"{this.username}: \uD83C\uDFB2 {message}");
        }

        private bool IsParsableArguments()
        {
            return this.IsArgumentsPassed() && Dice.CanParse(this.args[0]);
        }

        private bool IsArgumentsPassed()
        {
            return this.args.Count != 0;
        }
    }
}
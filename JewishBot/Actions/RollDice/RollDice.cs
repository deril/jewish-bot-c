namespace JewishBot.Actions.RollDice
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class RollDice : IAction
    {
        private const string DefaultPattern = "1d6";
        private readonly long chatId;
        private readonly string username;
        private readonly IBotService botService;
        private readonly ReadOnlyCollection<string> args;

        public RollDice(IBotService botService, long chatId, ReadOnlyCollection<string> args, string username)
        {
            this.botService = botService;
            this.chatId = chatId;
            this.args = args;
            this.username = username;
        }

        public async Task HandleAsync()
        {
            var toParse = this.IsParsableArguments() ? this.args[0] : DefaultPattern;
            var result = new Dice(toParse);

            var message = result.GetSum();

            await this.botService.Client.SendTextMessageAsync(this.chatId, $"{this.username}: \uD83C\uDFB2 {message}");
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
namespace JewishBot.Actions.RollDice
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class RollDice : IAction
    {
        private readonly ReadOnlyCollection<string> args;
        private readonly IBotService botService;
        private readonly long chatId;
        private readonly string username;

        public RollDice(IBotService botService, long chatId, ReadOnlyCollection<string> args, string username)
        {
            this.botService = botService;
            this.chatId = chatId;
            this.args = args;
            this.username = username;
        }

        public async Task HandleAsync()
        {
            if (!this.IsParsableArguments())
            {
                await this.botService.Client.SendDiceAsync(this.chatId);
                return;
            }

            var result = new Dice(this.args[0]);
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
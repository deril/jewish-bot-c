namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Services.DiceGame;

    internal class RollDice : IAction
    {
        private const string DefaultPatern = "1d6";
        private readonly long chatId;
        private readonly string[] args;
        private readonly string username;
        private readonly TelegramBotClient bot;

        public RollDice(TelegramBotClient bot, long chatId, string[] args, string username)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
            this.username = username;
        }

        public async Task HandleAsync()
        {
            var toParse = (this.args != null && Dice.CanParse(this.args[0])) ? this.args[0] : DefaultPatern;
            var result = new Dice(toParse);

            var message = result.GetSum().ToString();

            await this.bot.SendTextMessageAsync(this.chatId, $"{this.username}: \uD83C\uDFB2 {message}");
        }
    }
}
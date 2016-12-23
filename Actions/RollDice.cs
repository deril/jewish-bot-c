using JewishBot.Services.DiceGame;
using Telegram.Bot;

namespace JewishBot.Actions {
    public class RollDice : IAction {

        private const string DefaultPatern = "1d6";
        private readonly TelegramBotClient bot;
        private string message;
        
        public RollDice(TelegramBotClient bot) {
            this.bot = bot;
        }

        public async void HandleAsync(long chatId, string username, string[] args) {
            string toParse = (args != null && Dice.CanParse(args[0])) ? args[0] : DefaultPatern;
            Dice result = new Dice(toParse);

            message = result.GetSum().ToString();

            await bot.SendTextMessageAsync(chatId, $"{username}: \uD83C\uDFB2 {message}");
        }
    }
}
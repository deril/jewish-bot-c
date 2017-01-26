using JewishBot.Services.DiceGame;
using Telegram.Bot;

namespace JewishBot.Actions
{
    internal class RollDice : IAction
    {
        private const string DefaultPatern = "1d6";
        private readonly TelegramBotClient _bot;
        private string _message;

        public RollDice(TelegramBotClient bot)
        {
            _bot = bot;
        }

        public async void HandleAsync(long chatId, string username, string[] args)
        {
            var toParse = (args != null && Dice.CanParse(args[0])) ? args[0] : DefaultPatern;
            var result = new Dice(toParse);

            _message = result.GetSum().ToString();

            await _bot.SendTextMessageAsync(chatId, $"{username}: \uD83C\uDFB2 {_message}");
        }
    }
}
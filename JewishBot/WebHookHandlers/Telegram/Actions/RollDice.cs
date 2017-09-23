using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.DiceGame;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    class RollDice : IAction
    {
        const string DefaultPatern = "1d6";
        TelegramBotClient Bot { get; }
        long ChatId { get; }
        string[] Args { get; }
        string Username { get; }

        public RollDice(TelegramBotClient bot, long chatId, string[] args, string username)
        {
            Bot = bot;
            ChatId = chatId;
            Args = args;
            Username = username;
        }

        public async Task HandleAsync()
        {
            var toParse = (Args != null && Dice.CanParse(Args[0])) ? Args[0] : DefaultPatern;
            var result = new Dice(toParse);

            var message = result.GetSum().ToString();

            await Bot.SendTextMessageAsync(ChatId, $"{Username}: \uD83C\uDFB2 {message}");
        }
    }
}
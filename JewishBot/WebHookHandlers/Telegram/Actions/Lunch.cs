using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.Lunch;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    public class Lunch : IAction
    {
        readonly TelegramBotClient _bot;
        readonly long _chatId;
        readonly string[] _args;
        readonly IConfiguration _config;

        public Lunch(TelegramBotClient bot, long chatId, string[] args, IConfiguration config)
        {
            _bot = bot;
            _chatId = chatId;
            _args = args;
            _config = config;
        }

        public async Task HandleAsync()
        {
            var members = _args == null ? _config["lunch:members"] : string.Join("", _args);
            var lunchApi = new LunchApi(_config["lunch:email"], _config["lunch:password"], members);
            await _bot.SendTextMessageAsync(_chatId, lunchApi.Invoke());
        }
    }
}

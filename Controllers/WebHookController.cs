using JewishBot.WebHookHanders.Telegram;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace JewishBot.Controllers
{
    public class WebHookController : Controller {
        private readonly TelegramBotClient _bot;
        private readonly IConfiguration _configuration;

        public WebHookController(TelegramBotClient bot, IConfiguration configuration)
        {
            this._bot = bot;
            this._configuration = configuration;
        }

        public string Index()
        {
            return "hello, there";
        }

        [HttpPost]
        public StatusCodeResult Post([FromBody] Update update)
        {
            var message = update.Message;
            if (message == null || message.Type != MessageType.TextMessage) return NoContent();
            
            var handler = new WebHookHandler(_bot, _configuration);
            handler.OnMessageRecieved(message);

            return Ok();
        }
    }
}

using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JewishBot.Controllers
{
    public class WebHookController : Controller
    {
        readonly TelegramBotClient _bot;
        readonly IConfiguration _configuration;

        public WebHookController(TelegramBotClient bot, IConfiguration configuration)
        {
            _bot = bot;
            _configuration = configuration;
        }

        // GET: /WebHook/
        public string Index()
        {
            return "hello, there";
        }

        // POST: /WebHook/Post
        [HttpPost]
        public async Task<StatusCodeResult> Post([FromBody] Update update)
        {
            var message = update.Message;
            if (message == null || message.Type != MessageType.TextMessage) return NoContent();

            var handler = new WebHookHandler(_bot, _configuration);
            await handler.OnMessageReceived(message);

            return Ok();
        }

        // GET: /WebHook/Error
        public IActionResult Error() => View();
    }
}

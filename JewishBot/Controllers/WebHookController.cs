using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using JewishBot.WebHookHandlers.Telegram;
using Microsoft.Extensions.Logging;

namespace JewishBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebHookController : Controller
    {
        private readonly IWebHookHandler handler;
        private readonly ILogger<WebHookController> _logger;

        public WebHookController(IWebHookHandler webHook, ILogger<WebHookController> logger)
        {
            this.handler = webHook;
            _logger = logger;
        }

        // GET: /WebHook/
        [HttpGet]
        public string Index()
        {
            return "hello, there";
        }

        // POST: /WebHook/Post
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [Route("Post")]
        public async Task<StatusCodeResult> Post(Update update)
        {
            if (update is null) return this.BadRequest();

            var message = update.Message;
            if (CannotHandleMessage(message))
            {
                _logger.Log(LogLevel.Information, $"Cannot handle message {message}");
                return this.Ok();
            }

            await this.handler.OnMessageReceived(message).ConfigureAwait(true);

            return this.Ok();
        }

        private static bool CannotHandleMessage(Message message)
        {
            return IsEmptyMessage(message) || !IsTextMessage(message);
        }

        private static bool IsTextMessage(Message message)
        {
            return message.Type == MessageType.Text;
        }

        private static bool IsEmptyMessage(Message message)
        {
            return message == null;
        }
    }
}

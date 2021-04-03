namespace JewishBot.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using WebHookHandlers.Telegram;

    [ApiController]
    [Route("[controller]")]
    public class WebHookController : Controller
    {
        private readonly IWebHookHandler _handler;
        private readonly ILogger<WebHookController> _logger;

        public WebHookController(IWebHookHandler webHook, ILogger<WebHookController> logger)
        {
            _handler = webHook;
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
            if (update is null)
            {
                return BadRequest();
            }

            var message = update.Message;
            if (CannotHandleMessage(message))
            {
                return Ok();
            }

            _logger.LogWarning(
                $"Input for bot: Message from {message.Chat.Id}/{message.From.Username}, Text: {message.Text}");

            await _handler.OnMessageReceived(message).ConfigureAwait(true);

            return Ok();
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
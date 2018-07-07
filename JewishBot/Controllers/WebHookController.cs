namespace JewishBot.Controllers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using WebHookHandlers.Telegram;

    public class WebHookController : Controller
    {
        private WebHookHandler handler;

        public WebHookController(WebHookHandler webHook)
        {
            this.handler = webHook;
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
            if (message == null || message.Type != MessageType.Text)
            {
                return this.NoContent();
            }

            await this.handler.OnMessageReceived(message);

            return this.Ok();
        }

        // GET: /WebHook/Error
        public IActionResult Error() => this.BadRequest();
    }
}

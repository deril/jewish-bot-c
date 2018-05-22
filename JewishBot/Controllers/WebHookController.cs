namespace JewishBot.Controllers
{
    using System.Threading.Tasks;
    using JewishBot.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using WebHookHandlers.Telegram;

    public class WebHookController : Controller
    {
        private readonly TelegramBotClient bot;
        private readonly IConfiguration configuration;
        private IUserRepository repository;

        public WebHookController(TelegramBotClient bot, IConfiguration configuration, IUserRepository repo)
        {
            this.bot = bot;
            this.configuration = configuration;
            this.repository = repo;
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
            if (message == null || message.Type != MessageType.TextMessage)
            {
                return this.NoContent();
            }

            var handler = new WebHookHandler(this.bot, this.configuration, this.repository);
            await handler.OnMessageReceived(message);

            return this.Ok();
        }

        // GET: /WebHook/Error
        public IActionResult Error() => this.BadRequest();
    }
}

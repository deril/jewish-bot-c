using JewishBot.WebHookHandlers.Telegram;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

		public IActionResult Error() => View();
	}
}

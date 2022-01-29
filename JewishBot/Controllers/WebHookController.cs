using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace JewishBot.Controllers;

[ApiController]
[Route("[controller]")]
public class WebHookController : Controller
{
    private readonly IWebHookHandler _handler;

    public WebHookController(IWebHookHandler webHook)
    {
        _handler = webHook;
    }

    // GET: /WebHook/
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("hello there\n");
    }

    // POST: /WebHook/Post
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    [Route("Post")]
    public async Task<StatusCodeResult> Post(Update update)
    {
        var message = update.Message;

        await _handler.OnMessageReceived(message).ConfigureAwait(true);

        return Ok();
    }
}
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace JewishBot.WebHookHandlers.Telegram;

public interface IWebHookHandler
{
    Task OnMessageReceived(Message message);
}
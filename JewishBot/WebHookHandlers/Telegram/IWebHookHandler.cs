namespace JewishBot.WebHookHandlers.Telegram
{
    using System.Threading.Tasks;
    using global::Telegram.Bot.Types;

    public interface IWebHookHandler
    {
        Task OnMessageReceived(Message message);
    }
}
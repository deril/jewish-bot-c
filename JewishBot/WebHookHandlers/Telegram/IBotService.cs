using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace JewishBot.WebHookHandlers.Telegram;

public interface IBotService
{
    bool IsPrivateMode { get; }
    long PrivateChetId { get; }

    Task<Message> SendMessageAsync(string text, long chatId);
    Task<Message> SendDiceAsync(long chatId);
    Task<Message> SendLocationAsync(double latitude, double longitude, long chatId);
}
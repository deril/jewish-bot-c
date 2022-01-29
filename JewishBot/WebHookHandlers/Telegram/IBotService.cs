using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram;

public interface IBotService
{
    TelegramBotClient Client { get; }
    bool IsPrivateMode { get; }
    long PrivateChetId { get; }
}
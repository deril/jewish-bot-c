namespace JewishBot.WebHookHandlers.Telegram
{
    using global::Telegram.Bot;

    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}
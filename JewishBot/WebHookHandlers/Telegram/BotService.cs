using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram;

public class BotService : IBotService
{
    private readonly ILogger<BotService> _logger;

    public BotService(IOptions<BotConfiguration> config, ILogger<BotService> logger)
    {
        var configuration = config.Value;
        Client = new TelegramBotClient(configuration.BotToken);
        IsPrivateMode = configuration.PrivateMode;
        PrivateChetId = configuration.PrivateChetId;
        _logger = logger;

        // UpdateWebHook(configuration);
    }

    public TelegramBotClient Client { get; }
    public bool IsPrivateMode { get; }
    public long PrivateChetId { get; }

    private void UpdateWebHook(BotConfiguration configuration)
    {
        var webHookUrl = $"https://{configuration.HostN}/WebHook/Post";
        var webHookInfo = Client.GetWebhookInfoAsync().GetAwaiter();
        var setWebHookUrl = webHookInfo.GetResult().Url;
        _logger.LogWarning("Webhook set on the server: {SetWebHookUrl}", setWebHookUrl);
        if (setWebHookUrl == webHookUrl) return;

        Client.SetWebhookAsync(webHookUrl).Wait();
    }
}
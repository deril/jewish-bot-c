using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JewishBot.WebHookHandlers.Telegram;

public class BotService : IBotService
{
    private const string MissingTokenErrorMessage = "Missing Telegram Bot Token";
    private readonly ILogger<BotService> _logger;
    private readonly ITelegramBotClient _botClient;

    public BotService(IOptions<BotConfiguration> config, ILogger<BotService> logger)
    {
        var configuration = config.Value;
        if (string.IsNullOrEmpty(configuration.BotToken)) throw new ArgumentException(MissingTokenErrorMessage);

        _botClient = new TelegramBotClient(configuration.BotToken);
        IsPrivateMode = configuration.PrivateMode;
        PrivateChetId = configuration.PrivateChetId;
        _logger = logger;

        UpdateWebHook(configuration);
    }

    public bool IsPrivateMode { get; }
    public long PrivateChetId { get; }

    public Task<Message> SendMessageAsync(string text, long chatId)
    {
        return _botClient.SendTextMessageAsync(chatId, text);
    }

    public Task<Message> SendDiceAsync(long chatId)
    {
        return _botClient.SendDiceAsync(chatId);
    }

    public Task<Message> SendLocationAsync(double latitude, double longitude, long chatId)
    {
        return _botClient.SendLocationAsync(chatId, latitude, longitude);
    }

    private void UpdateWebHook(BotConfiguration configuration)
    {
        var webHookUrl = $"https://{configuration.HostN}/WebHook/Post";
        var webHookInfo = _botClient.GetWebhookInfoAsync().GetAwaiter();
        var setWebHookUrl = webHookInfo.GetResult().Url;
        _logger.LogWarning("Webhook set on the server: {SetWebHookUrl}", setWebHookUrl);
        if (setWebHookUrl == webHookUrl) return;

        _botClient.SetWebhookAsync(webHookUrl).Wait();
    }
}
namespace JewishBot.WebHookHandlers.Telegram
{
    using System;
    using global::Telegram.Bot;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class BotService : IBotService
    {
        public BotService(IOptions<BotConfiguration> config, ILogger<BotService> logger)
        {
            if (config is null)
            {
                throw new ArgumentNullException($"{nameof(config)} cannot be null");
            }

            var configuration = config.Value;
            Client = new TelegramBotClient(configuration.BotToken);
            IsPrivateMode = configuration.PrivateMode;
            PrivateChetId = configuration.PrivateChetId;

            UpdateWebHook(logger, configuration);
        }

        private void UpdateWebHook(ILogger logger, BotConfiguration configuration)
        {
            var webHookUrl = $"https://{configuration.HostN}/WebHook/Post";
            var webHookInfo = Client.GetWebhookInfoAsync().GetAwaiter();
            var setWebHookUrl = webHookInfo.GetResult().Url;
            logger.LogWarning("Webhook set on the server: {SetWebHookUrl}", setWebHookUrl);
            if (setWebHookUrl == webHookUrl)
            {
                return;
            }

            Client.SetWebhookAsync(webHookUrl).Wait();
        }

        public TelegramBotClient Client { get; }
        public bool IsPrivateMode { get; }
        public long PrivateChetId { get; }
    }
}
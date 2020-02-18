namespace JewishBot.WebHookHandlers.Telegram
{
    using System;
    using System.IO;
    using global::Telegram.Bot;
    using Microsoft.Extensions.Options;

    public class BotService : IBotService
    {
        public BotService(IOptions<BotConfiguration> config)
        {
            if (config is null)
            {
                throw new ArgumentNullException($"{nameof(config)} cannot be null");
            }

            var configuration = config.Value;
            this.Client = new TelegramBotClient(configuration.BotToken);

            var webHookUrl = $"https://{configuration.HostN}/WebHook/Post";
            var webHookInfo = this.Client.GetWebhookInfoAsync().GetAwaiter();
            if (webHookInfo.GetResult().Url == webHookUrl)
            {
                return;
            }

            using var certificate = File.OpenRead("jewish_bot.pem");
            this.Client.SetWebhookAsync(webHookUrl, certificate).Wait();
        }

        public TelegramBotClient Client { get; }
    }
}
namespace JewishBot.WebHookHandlers.Telegram
{
    using System.IO;
    using global::Telegram.Bot;
    using Microsoft.Extensions.Options;

    public class BotService : IBotService
    {
        private const string WebHookUrl = "https://{configuration.HostN}/WebHook/Post";

        public BotService(IOptions<BotConfiguration> config)
        {
            var configuration = config.Value;
            this.Client = new TelegramBotClient(configuration.BotToken);

            var webHookInfo = this.Client.GetWebhookInfoAsync().GetAwaiter();
            if (webHookInfo.GetResult().Url == WebHookUrl)
            {
                return;
            }

            using (var certificate = File.OpenRead("jewish_bot.pem"))
            {
                this.Client.SetWebhookAsync($"https://{configuration.HostN}/WebHook/Post", certificate).Wait();
            }
        }

        public TelegramBotClient Client { get; }
    }
}
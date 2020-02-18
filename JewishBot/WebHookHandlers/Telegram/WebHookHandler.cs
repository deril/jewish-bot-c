namespace JewishBot.WebHookHandlers.Telegram
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using global::Telegram.Bot.Types;
    using JewishBot.Actions;
    using JewishBot.Actions.DuckDuckGo;
    using JewishBot.Actions.GoogleMaps;
    using JewishBot.Actions.RollDice;
    using JewishBot.Actions.UrbanDictionary;
    using JewishBot.Actions.Weather;
    using Microsoft.Extensions.Configuration;

    public class WebHookHandler : IWebHookHandler
    {
        private readonly IBotService botService;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;

        public WebHookHandler(IBotService botService, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            this.botService = botService;
            this.configuration = configuration;
            this.clientFactory = clientFactory;
        }

        public async Task OnMessageReceived(Message message)
        {
            var command = new CommandParser(message.Text).Parse();
            var chatId = message.Chat.Id;
            var username = string.IsNullOrEmpty(message.From.Username)
                ? $"{message.From.FirstName} {message.From.LastName}"
                : message.From.Username;

            IAction cmd = command.Name switch
            {
                "echo" => new Echo(this.botService, chatId, command.Arguments),
                "hey" => new Hey(this.botService, chatId),
                "ex" => new CurrencyExchange(this.botService, chatId, command.Arguments,
                    this.configuration["apiForexKey"]),
                "ud" => new UrbanDictionary(this.botService, this.clientFactory, chatId, command.Arguments),
                "go" => new DuckDuckGo(this.botService, this.clientFactory, chatId, command.Arguments),
                "dice" => new RollDice(this.botService, chatId, command.Arguments, username),
                "l" => new GoogleMaps(this.botService, this.clientFactory, chatId, command.Arguments,
                    this.configuration["googleApiKey"]),
                "weekday" => new WeekDay(this.botService, chatId),
                "timein" => new TimeInPlace(this.botService, this.clientFactory, chatId, command.Arguments,
                    this.configuration["googleApiKey"]),
                "ball" => new MagicBall(this.botService, chatId, command.Arguments),
                "weather" => new Weather(this.botService, this.clientFactory, chatId, command.Arguments),
                _ => new NoCommand()
            };

            await cmd.HandleAsync();
        }
    }
}
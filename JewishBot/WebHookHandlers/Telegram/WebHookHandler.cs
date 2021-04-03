namespace JewishBot.WebHookHandlers.Telegram
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Actions;
    using Actions.DuckDuckGo;
    using Actions.GoogleMaps;
    using Actions.RollDice;
    using Actions.UrbanDictionary;
    using Actions.Weather;
    using global::Telegram.Bot.Types;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class WebHookHandler : IWebHookHandler
    {
        private readonly IBotService _botService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WebHookHandler> _logger;

        public WebHookHandler(IBotService botService, IConfiguration configuration, IHttpClientFactory clientFactory,
            ILogger<WebHookHandler> logger)
        {
            _botService = botService;
            _configuration = configuration;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task OnMessageReceived(Message message)
        {
            var command = new CommandParser(message.Text).Parse();
            var chatId = message.Chat.Id;
            var username = string.IsNullOrEmpty(message.From.Username)
                ? $"{message.From.FirstName} {message.From.LastName}"
                : message.From.Username;

            IAction cmd;
            if (_botService.IsPrivateMode && chatId != _botService.PrivateChetId)
            {
                cmd = new NoCommand();
            }
            else
            {
                cmd = command.Name switch
                {
                    "echo" => new Echo(_botService, chatId, command.Arguments),
                    "hey" => new Hey(_botService, chatId),
                    "ex" => new CurrencyExchange(_botService, chatId, command.Arguments, _configuration["apiForexKey"]),
                    "ud" => new UrbanDictionary(_botService, _clientFactory, chatId, command.Arguments),
                    "go" => new DuckDuckGo(_botService, _clientFactory, chatId, command.Arguments),
                    "dice" => new RollDice(_botService, chatId, command.Arguments, username),
                    "l" => new GoogleMaps(_botService, _clientFactory, chatId, command.Arguments,
                        _configuration["googleApiKey"]),
                    "weekday" => new WeekDay(_botService, chatId),
                    "timein" => new TimeInPlace(_botService, _clientFactory, chatId, command.Arguments,
                        _configuration["googleApiKey"]),
                    "ball" => new MagicBall(_botService, chatId, command.Arguments),
                    "weather" => new Weather(_botService, _clientFactory, chatId, command.Arguments),
                    _ => new NoCommand()
                };
            }

            try
            {
                await cmd.HandleAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot execute command, error {Message}", e.Message);
            }
        }
    }
}
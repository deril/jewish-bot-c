namespace JewishBot.WebHookHandlers.Telegram
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Actions;
    using Data;
    using global::Telegram.Bot.Types;
    using Microsoft.Extensions.Configuration;

    public class WebHookHandler : IWebHookHandler
    {
        private readonly IBotService botService;
        private readonly IConfiguration configuration;
        private readonly IUserRepository repository;
        private readonly IHttpClientFactory clientFactory;

        public WebHookHandler(IBotService botService, IConfiguration configuration, IUserRepository repo, IHttpClientFactory clientFactory)
        {
            this.botService = botService;
            this.configuration = configuration;
            this.repository = repo;
            this.clientFactory = clientFactory;
        }

        public async Task OnMessageReceived(Message message)
        {
            var command = new CommandParser(message.Text).Parse();
            var chatId = message.Chat.Id;
            var chatType = message.Chat.Type;
            var userId = message.From.Id;
            var username = string.IsNullOrEmpty(message.From.Username)
                                 ? $"{message.From.FirstName} {message.From.LastName}"
                                 : message.From.Username;

            var commands = new Dictionary<string, IAction>()
            {
                { "echo",       new Echo(this.botService, chatId, command.Arguments) },
                { "hey",        new Hey(this.botService, chatId) },
                { "ex",         new CurrencyExchange(this.botService, chatId, command.Arguments, this.configuration["apiForexKey"]) },
                { "ud",         new UrbanDictionary(this.botService, this.clientFactory, chatId, command.Arguments) },
                { "go",         new DuckDuckGo(this.botService, this.clientFactory, chatId, command.Arguments) },
                { "dice",       new RollDice(this.botService, chatId, command.Arguments, username) },
//                { "poem",       new Poem(this.botService, this.clientFactory, chatId) },
                { "l",          new GoogleMaps(this.botService, this.clientFactory, chatId, command.Arguments, this.configuration["googleApiKey"]) },
                { "advice",     new Advice(this.botService, this.clientFactory, chatId, username) },
                { "weekday",    new WeekDay(this.botService, chatId) },
                { "timein",     new TimeInPlace(this.botService, this.clientFactory, chatId, command.Arguments, this.configuration["googleApiKey"]) },
//                { "calc",       new Calc(this.botService, this.clientFactory, chatId, command.Arguments) },
                { "ball",       new MagicBall(this.botService, chatId, command.Arguments) },
                { "lunch",      new Lunch(this.botService, chatId, userId, chatType, command.Arguments, this.configuration, this.repository, this.clientFactory) },
                { "setlunch",   new SetLunch(this.botService, chatId, userId, chatType, command.Arguments, this.repository) }
            };

            if (commands.ContainsKey(command.Name))
            {
                await commands[command.Name].HandleAsync();
            }
        }
    }
}
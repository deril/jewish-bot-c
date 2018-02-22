namespace JewishBot.WebHookHandlers.Telegram
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Actions;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using Microsoft.Extensions.Configuration;
    using Services.GoogleFinance;

    public class WebHookHandler
    {
        public WebHookHandler(TelegramBotClient bot, IConfiguration configuration)
        {
            this.Bot = bot;
            this.Configuration = configuration;
        }

        private TelegramBotClient Bot { get; }

        private IConfiguration Configuration { get; }

        public async Task OnMessageReceived(Message message)
        {
            var command = new CommandParser(message.Text).Parse();
            var chatId = message.Chat.Id;
            var username = string.IsNullOrEmpty(message.From.Username)
                                 ? $"{message.From.FirstName} {message.From.LastName}"
                                 : message.From.Username;

            var commands = new Dictionary<string, IAction>()
            {
                { "echo",       new Echo(this.Bot, chatId, command.Arguments) },
                { "hey",        new Hey(this.Bot, chatId) },
                { "ex",         new CurrencyExchange(this.Bot, chatId, command.Arguments, new FinanceApi()) },
                { "ud",         new UrbanDictionary(this.Bot, chatId, command.Arguments) },
                { "go",         new DuckDuckGo(this.Bot, chatId, command.Arguments) },
                { "dice",       new RollDice(this.Bot, chatId, command.Arguments, username) },

                // { "poem",       new Poem(Bot, chatId) },
                { "l",          new GoogleMaps(this.Bot, chatId, command.Arguments, this.Configuration["googleApiKey"]) },
                { "advice",     new Advice(this.Bot, chatId, username) },
                { "weekday",    new WeekDay(this.Bot, chatId) },
                { "timein",     new TimeInPlace(this.Bot, chatId, command.Arguments, this.Configuration["googleApiKey"]) },
                { "calc",       new Calc(this.Bot, chatId, command.Arguments) },
                { "ball",       new MagicBall(this.Bot, chatId, command.Arguments) },
                { "lunch",      new Lunch(this.Bot, chatId, command.Arguments, this.Configuration) }
            };

            if (commands.ContainsKey(command.Name))
            {
                await commands[command.Name].HandleAsync();
            }
        }
    }
}
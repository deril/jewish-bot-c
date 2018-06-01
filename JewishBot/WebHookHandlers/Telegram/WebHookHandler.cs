﻿namespace JewishBot.WebHookHandlers.Telegram
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Actions;
    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using JewishBot.Data;
    using Microsoft.Extensions.Configuration;

    public class WebHookHandler
    {
        private readonly TelegramBotClient bot;
        private readonly IConfiguration configuration;
        private readonly IUserRepository repository;

        public WebHookHandler(TelegramBotClient bot, IConfiguration configuration, IUserRepository repo)
        {
            this.bot = bot;
            this.configuration = configuration;
            this.repository = repo;
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
                { "echo",       new Echo(this.bot, chatId, command.Arguments) },
                { "hey",        new Hey(this.bot, chatId) },
                { "ex",         new CurrencyExchange(this.bot, chatId, command.Arguments, this.configuration["apiForexKey"]) },
                { "ud",         new UrbanDictionary(this.bot, chatId, command.Arguments) },
                { "go",         new DuckDuckGo(this.bot, chatId, command.Arguments) },
                { "dice",       new RollDice(this.bot, chatId, command.Arguments, username) },
                { "poem",       new Poem(this.bot, chatId) },
                { "l",          new GoogleMaps(this.bot, chatId, command.Arguments, this.configuration["googleApiKey"]) },
                { "advice",     new Advice(this.bot, chatId, username) },
                { "weekday",    new WeekDay(this.bot, chatId) },
                { "timein",     new TimeInPlace(this.bot, chatId, command.Arguments, this.configuration["googleApiKey"]) },
                { "calc",       new Calc(this.bot, chatId, command.Arguments) },
                { "ball",       new MagicBall(this.bot, chatId, command.Arguments) },
                { "lunch",      new Lunch(this.bot, chatId, userId, chatType, command.Arguments, this.configuration, this.repository) },
                { "setlunch",   new SetLunch(this.bot, chatId, userId, chatType, command.Arguments, this.repository) }
            };

            if (commands.ContainsKey(command.Name))
            {
                await commands[command.Name].HandleAsync();
            }
        }
    }
}
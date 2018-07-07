namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;
    using GeoTimeZone;
    using global::Telegram.Bot;
    using Services.GoogleMaps;

    internal class TimeInPlace : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly string apiKey;
        private readonly IHttpClientFactory clientFactory;
        private IReadOnlyCollection<string> args;

        public TimeInPlace(TelegramBotClient bot, IHttpClientFactory clientFactory, long chatId, IReadOnlyCollection<string> args, string key)
        {
            this.bot = bot;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
            this.args = args;
            this.apiKey = key;
        }

        private enum Status
        {
            Ok,
            Error
        }

        public static string Description { get; } = @"Returns time in specified location
Usage: /timein location";

        public async Task HandleAsync()
        {
            if (this.args == null)
            {
                await this.bot.SendTextMessageAsync(this.chatId, Description);
                return;
            }

            var locationResult = await this.GetLocationAsync(this.args);
            if (locationResult.Item1 == Status.Error)
            {
                const string errorMessage = "Something goes wrong \uD83D\uDE22";
                await this.bot.SendTextMessageAsync(this.chatId, errorMessage);
                return;
            }

            var time = GetTimeInLocation(locationResult.Item2);
            await this.bot.SendTextMessageAsync(this.chatId, $"In {string.Join(" ", this.args)}: {time}");
        }

        private static string GetTimeInLocation(Location location)
        {
            var timeZone = TimeZoneLookup.GetTimeZone(location.Lattitude, location.Longtitude).Result;
            var culture = new CultureInfo("uk-UA", true);

            return TimeZoneInfo.ConvertTime(DateTimeOffset.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone))
                               .ToString("t", culture);
        }

        private async Task<Tuple<Status, Location>> GetLocationAsync(IReadOnlyCollection<string> place)
        {
            var mapsApi = new GoogleMapsApi(this.clientFactory, this.apiKey);
            var response = await mapsApi.InvokeAsync(place);

            return response.Status == "OK"
                ? new Tuple<Status, Location>(Status.Ok, response.Results[0].Geometry.Location)
                : new Tuple<Status, Location>(Status.Error, null);
        }
    }
}
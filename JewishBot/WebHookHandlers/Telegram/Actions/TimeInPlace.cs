namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System;
    using System.Threading.Tasks;
    using GeoTimeZone;
    using global::Telegram.Bot;
    using Services.GoogleMaps;

    public enum Status
    {
        Ok,
        Error
    }

    internal class TimeInPlace : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly string[] args;
        private readonly GoogleMapsApi mapsApi;

        public TimeInPlace(TelegramBotClient bot, long chatId, string[] args, string key)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
            this.mapsApi = new GoogleMapsApi(key);
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

            var location = string.Join(" ", this.args);
            var locationResult = await this.GetLocationAsync(location);
            if (locationResult.Item1 == Status.Error)
            {
                const string errorMessage = "Something goes wrong \uD83D\uDE22";
                await this.bot.SendTextMessageAsync(this.chatId, errorMessage);
                return;
            }

            var time = GetTimeInLocation(locationResult.Item2);
            await this.bot.SendTextMessageAsync(this.chatId, $"In {location}: {time}");
        }

        private static string GetTimeInLocation(Location location)
        {
            var timeZone = TimeZoneLookup.GetTimeZone(location.Lattitude, location.Longtitude).Result;

            return TimeZoneInfo.ConvertTime(DateTimeOffset.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone))
                .ToString("t");
        }

        private async Task<Tuple<Status, Location>> GetLocationAsync(string place)
        {
            var response = await this.mapsApi.InvokeAsync<QueryModel>(new[] { place });

            return response.Status == "OK"
                ? new Tuple<Status, Location>(Status.Ok, response.Results[0].Geometry.Location)
                : new Tuple<Status, Location>(Status.Error, null);
        }
    }
}
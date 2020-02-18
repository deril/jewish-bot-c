namespace JewishBot.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;
    using GeoTimeZone;
    using GoogleMaps;
    using WebHookHandlers.Telegram;

    internal class TimeInPlace : IAction
    {
        private const string Description = @"Returns time in specified location
Usage: /timein location";

        private readonly string apiKey;
        private readonly IReadOnlyCollection<string> args;

        private readonly IBotService botService;
        private readonly long chatId;
        private readonly IHttpClientFactory clientFactory;

        public TimeInPlace(IBotService botService, IHttpClientFactory clientFactory, long chatId,
            IReadOnlyCollection<string> args, string key)
        {
            this.botService = botService;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
            this.args = args;
            this.apiKey = key;
        }

        public async Task HandleAsync()
        {
            if (this.args == null)
            {
                await this.botService.Client.SendTextMessageAsync(this.chatId, Description);
                return;
            }

            var (status, location) = await this.GetLocationAsync(this.args);
            if (status == Status.Error)
            {
                const string errorMessage = "Something goes wrong \uD83D\uDE22";
                await this.botService.Client.SendTextMessageAsync(this.chatId, errorMessage);
                return;
            }

            var time = GetTimeInLocation(location);
            await this.botService.Client.SendTextMessageAsync(this.chatId, $"In {string.Join(" ", this.args)}: {time}");
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

        private enum Status
        {
            Ok,
            Error
        }
    }
}
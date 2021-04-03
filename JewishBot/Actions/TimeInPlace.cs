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

        private readonly string _apiKey;
        private readonly IReadOnlyCollection<string> _args;

        private readonly IBotService _botService;
        private readonly long _chatId;
        private readonly IHttpClientFactory _clientFactory;

        public TimeInPlace(IBotService botService, IHttpClientFactory clientFactory, long chatId,
            IReadOnlyCollection<string> args, string key)
        {
            _botService = botService;
            _clientFactory = clientFactory;
            _chatId = chatId;
            _args = args;
            _apiKey = key;
        }

        public async Task HandleAsync()
        {
            if (_args == null)
            {
                await _botService.Client.SendTextMessageAsync(_chatId, Description);
                return;
            }

            var (status, location) = await GetLocationAsync(_args);
            if (status == Status.Error)
            {
                const string errorMessage = "Something goes wrong \uD83D\uDE22";
                await _botService.Client.SendTextMessageAsync(_chatId, errorMessage);
                return;
            }

            var time = GetTimeInLocation(location);
            await _botService.Client.SendTextMessageAsync(_chatId, $"In {string.Join(" ", _args)}: {time}");
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
            var mapsApi = new GoogleMapsApi(_clientFactory, _apiKey);
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
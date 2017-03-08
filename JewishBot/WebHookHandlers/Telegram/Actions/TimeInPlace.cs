using System;
using System.Threading.Tasks;
using GeoTimeZone;
using JewishBot.WebHookHandlers.Telegram.Services.GoogleMaps;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    public enum Status
    {
        Ok,
        Error
    }

    class TimeInPlace : IAction
    {
		readonly string _key;
		TelegramBotClient Bot { get; }

		public static string Description { get; } = @"Returns time in specified location
Usage: /timein location";

        public TimeInPlace(TelegramBotClient bot, string key)
        {
            Bot = bot;
			_key = key;
        }

		public async Task HandleAsync(long chatId, string[] args)
        {
            if (args == null)
            {
                await Bot.SendTextMessageAsync(chatId, Description);
                return;
            }
            var location = string.Join(" ", args);
            var locationResult = await GetLocation(location);
            if (locationResult.Item1 == Status.Error)
            {
                const string errorMessage = "Something goes wrong \uD83D\uDE22";
                await Bot.SendTextMessageAsync(chatId, errorMessage);
                return;
            }
            var time = GetTimeInLocation(locationResult.Item2);
            await Bot.SendTextMessageAsync(chatId, $"In {location}: {time}");
        }

		async Task<Tuple<Status, Location>> GetLocation(string place)
		{
			var mapsApi = new GoogleMapsApi(_key);
			var response = await mapsApi.Invoke<QueryModel>(place);

			return response.Status == "OK"
				? new Tuple<Status, Location>(Status.Ok, response.Results[0].Geometry.Location)
				: new Tuple<Status, Location>(Status.Error, null);
		}

		static string GetTimeInLocation(Location location)
		{
			var timeZone = TimeZoneLookup.GetTimeZone(location.Lattitude, location.Longtitude).Result;

			return TimeZoneInfo.ConvertTime(DateTimeOffset.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone))
				.ToString("t");
		}
	}
}
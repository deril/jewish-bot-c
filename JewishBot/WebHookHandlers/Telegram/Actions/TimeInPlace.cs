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
		long ChatId { get; }
		string[] Args { get; }

		public static string Description { get; } = @"Returns time in specified location
Usage: /timein location";

		public TimeInPlace(TelegramBotClient bot, long chatId, string[] args, string key)
		{
			Bot = bot;
			ChatId = chatId;
			Args = args;
			_key = key;
		}

		public async Task HandleAsync()
		{
			if (Args == null)
			{
				await Bot.SendTextMessageAsync(ChatId, Description);
				return;
			}
			var location = string.Join(" ", Args);
			var locationResult = await GetLocation(location);
			if (locationResult.Item1 == Status.Error)
			{
				const string errorMessage = "Something goes wrong \uD83D\uDE22";
				await Bot.SendTextMessageAsync(ChatId, errorMessage);
				return;
			}
			var time = GetTimeInLocation(locationResult.Item2);
			await Bot.SendTextMessageAsync(ChatId, $"In {location}: {time}");
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
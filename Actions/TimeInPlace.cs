﻿using System;
using System.Threading.Tasks;
using GeoTimeZone;
using JewishBot.Services.GoogleMaps;
using Telegram.Bot;

namespace JewishBot.Actions
{
    public enum Status
    {
        Ok,
        Error
    }

    public class TimeInPlace : IAction
    {
        private TelegramBotClient Bot { get; }
        private const string DefaultTimeZone = "Europe/Kiev";
        public static string Description { get; } = @"Returns time in specified location
Usage: /timein location";

        public TimeInPlace(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args)
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

        private static async Task<Tuple<Status, Location>> GetLocation(string place)
        {
            var mapsApi = new GoogleMapsApi();
            var response = await mapsApi.Invoke<QueryModel>(place);

            return response.Status == "OK" ?
                new Tuple<Status, Location>(Status.Ok, response.Results[0].Geometry.Location) :
                new Tuple<Status, Location>(Status.Error, null);
        }

        private static string GetTimeInLocation(Location location)
        {
            var timeZone = TimeZoneLookup.GetTimeZone(location.Lattitude, location.Longtitude).Result;
            var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now,
                TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZone));

            return TimeZoneInfo.ConvertTime(currentTime, TimeZoneInfo.FindSystemTimeZoneById(timeZone)).ToString("t");
        }
    }
}
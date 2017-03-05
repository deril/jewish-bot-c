﻿using JewishBot.WebHookHandlers.Telegram.Services.GoogleMaps;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    internal class GoogleMaps
    {
        private readonly string _key;
        private TelegramBotClient Bot { get; }

        public GoogleMaps(TelegramBotClient bot, string key)
        {
            Bot = bot;
            this._key = key;
        }

        public async void HandleAsync(long chatId, string[] args)
        {
            var message = "Please specify an address";
            if (args == null)
            {
                await Bot.SendTextMessageAsync(chatId, message);
                return;
            }

            var mapsApi = new GoogleMapsApi(_key);
            var response = await mapsApi.Invoke<QueryModel>(string.Join(" ", args));

            if (response.Status != "OK")
            {
                // TODO: implement here logging
                message = "Nothing \uD83D\uDE22";
                await Bot.SendTextMessageAsync(chatId, message);
                return;
            }

            var location = response.Results[0].Geometry.Location;
            await Bot.SendLocationAsync(chatId, location.Lattitude, location.Longtitude);
        }
    }
}
﻿namespace JewishBot.Actions.GoogleMaps
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class GoogleMaps : IAction
    {
        private readonly string _apiKey;
        private readonly IReadOnlyCollection<string> _args;
        private readonly IBotService _botService;
        private readonly long _chatId;
        private readonly IHttpClientFactory _clientFactory;

        public GoogleMaps(IBotService botService, IHttpClientFactory clientFactory, long chatId,
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
            var message = "Please specify an address";
            if (_args == null)
            {
                await _botService.Client.SendTextMessageAsync(_chatId, message);
                return;
            }

            var mapsApi = new GoogleMapsApi(_clientFactory, _apiKey);
            var response = await mapsApi.InvokeAsync(_args);

            if (response.Status != "OK")
            {
                message = "Nothing \uD83D\uDE22";
                await _botService.Client.SendTextMessageAsync(_chatId, message);
                return;
            }

            var location = response.Results[0].Geometry.Location;
            await _botService.Client.SendLocationAsync(_chatId, location.Lattitude, location.Longtitude);
        }
    }
}
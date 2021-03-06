namespace JewishBot.Actions.Weather
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class Weather : IAction
    {
        private readonly IReadOnlyCollection<string> _args;
        private readonly IBotService _botService;
        private readonly long _chatId;
        private readonly IHttpClientFactory _clientFactory;

        public Weather(IBotService botService, IHttpClientFactory clientFactory, long chatId,
            IReadOnlyCollection<string> args)
        {
            _botService = botService;
            _clientFactory = clientFactory;
            _chatId = chatId;
            _args = args;
        }

        public async Task HandleAsync()
        {
            if (_args == null)
            {
                await _botService.Client.SendTextMessageAsync(_chatId, "Please specify a city or region");
                return;
            }

            var weatherApi = new WeatherApi(_clientFactory);
            var response = await weatherApi.InvokeAsync(_args);
            if (string.IsNullOrEmpty(response))
            {
                await _botService.Client.SendTextMessageAsync(_chatId, "Nothing \uD83D\uDE22");
                return;
            }

            await _botService.Client.SendTextMessageAsync(_chatId, response);
        }
    }
}
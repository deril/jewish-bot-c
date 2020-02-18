namespace JewishBot.Actions.Weather
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class Weather : IAction
    {
        private readonly IBotService botService;
        private readonly long chatId;
        private readonly IHttpClientFactory clientFactory;
        private readonly IReadOnlyCollection<string> args;

        public Weather(IBotService botService, IHttpClientFactory clientFactory, long chatId, IReadOnlyCollection<string> args)
        {
            this.botService = botService;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
            this.args = args;
        }

        public async Task HandleAsync()
        {
            if (this.args == null)
            {
                await this.botService.Client.SendTextMessageAsync(this.chatId, "Please specify a city or region");
                return;
            }
            var weatherApi = new WeatherApi(this.clientFactory);
            var response = await weatherApi.InvokeAsync(this.args);
            if (string.IsNullOrEmpty(response))
            {
                await this.botService.Client.SendTextMessageAsync(this.chatId, "Nothing \uD83D\uDE22");
                return;
            }
            await this.botService.Client.SendTextMessageAsync(this.chatId, response);
        }
    }
}

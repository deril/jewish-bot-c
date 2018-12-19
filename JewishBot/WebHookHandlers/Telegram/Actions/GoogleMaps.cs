namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Services.GoogleMaps;

    internal class GoogleMaps : IAction
    {
        private readonly IBotService botService;
        private readonly long chatId;
        private readonly IHttpClientFactory clientFactory;
        private readonly string apiKey;
        private readonly IReadOnlyCollection<string> args;

        public GoogleMaps(IBotService botService, IHttpClientFactory clientFactory, long chatId, IReadOnlyCollection<string> args, string key)
        {
            this.botService = botService;
            this.clientFactory = clientFactory;
            this.chatId = chatId;
            this.args = args;
            this.apiKey = key;
        }

        public async Task HandleAsync()
        {
            var message = "Please specify an address";
            if (this.args == null)
            {
                await this.botService.Client.SendTextMessageAsync(this.chatId, message);
                return;
            }

            var mapsApi = new GoogleMapsApi(this.clientFactory, this.apiKey);
            var response = await mapsApi.InvokeAsync(this.args);

            if (response.Status != "OK")
            {
                message = "Nothing \uD83D\uDE22";
                await this.botService.Client.SendTextMessageAsync(this.chatId, message);
                return;
            }

            var location = response.Results[0].Geometry.Location;
            await this.botService.Client.SendLocationAsync(this.chatId, location.Lattitude, location.Longtitude);
        }
    }
}
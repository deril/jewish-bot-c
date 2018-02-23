namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System.Threading.Tasks;
    using global::Telegram.Bot;
    using Services.GoogleMaps;

    internal class GoogleMaps : IAction
    {
        private readonly TelegramBotClient bot;
        private readonly long chatId;
        private readonly string[] args;
        private readonly GoogleMapsApi mapsApi;

        public GoogleMaps(TelegramBotClient bot, long chatId, string[] args, string key)
        {
            this.bot = bot;
            this.chatId = chatId;
            this.args = args;
            this.mapsApi = new GoogleMapsApi(key);
        }

        public async Task HandleAsync()
        {
            var message = "Please specify an address";
            if (this.args == null)
            {
                await this.bot.SendTextMessageAsync(this.chatId, message);
                return;
            }

            var response = await this.mapsApi.InvokeAsync<QueryModel>(new string[] { string.Join(" ", this.args) });

            if (response.Status != "OK")
            {
                // TODO: implement here logging
                message = "Nothing \uD83D\uDE22";
                await this.bot.SendTextMessageAsync(this.chatId, message);
                return;
            }

            var location = response.Results[0].Geometry.Location;
            await this.bot.SendLocationAsync(this.chatId, location.Lattitude, location.Longtitude);
        }
    }
}
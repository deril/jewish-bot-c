using JewishBot.Services.GoogleMaps;
using Telegram.Bot;

namespace JewishBot.Actions
{
    internal class GoogleMaps
    {
        private TelegramBotClient Bot { get; }
        private GoogleMapsApi MapsApi { get; } = new GoogleMapsApi();
        private string Message { get; set; } = "Please specify an address";

        public GoogleMaps(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async void HandleAsync(long chatId, string[] args)
        {
            if (args == null)
            {
                await Bot.SendTextMessageAsync(chatId, Message);
                return;
            }

            var response = await MapsApi.Invoke<QueryModel>(string.Join(" ", args));

            if (response.Status != "OK")
            {
                // TODO: implement here logging
                Message = "Nothing \uD83D\uDE22";
                await Bot.SendTextMessageAsync(chatId, Message);
                return;
            }

            var location = response.Results[0].Geometry.Location;
            await Bot.SendLocationAsync(chatId, location.Lattitude, location.Longtitude);
        }
    }
}
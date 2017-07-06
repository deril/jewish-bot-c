using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram.Services.GoogleMaps;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    class GoogleMaps : IAction
	{
        TelegramBotClient Bot { get; }
        long ChatId { get; }
        string[] Args { get; }
		
        readonly string _key;

        public GoogleMaps(TelegramBotClient bot, long chatId, string[] args, string key)
		{
			Bot = bot;
            ChatId = chatId;
            Args = args;
			_key = key;
		}

		public async Task HandleAsync()
		{
			var message = "Please specify an address";
			if (Args == null)
			{
				await Bot.SendTextMessageAsync(ChatId, message);
				return;
			}

			var mapsApi = new GoogleMapsApi(_key);
			var response = await mapsApi.Invoke<QueryModel>(string.Join(" ", Args));

			if (response.Status != "OK")
			{
				// TODO: implement here logging
				message = "Nothing \uD83D\uDE22";
				await Bot.SendTextMessageAsync(ChatId, message);
				return;
			}

			var location = response.Results[0].Geometry.Location;
			await Bot.SendLocationAsync(ChatId, location.Lattitude, location.Longtitude);
		}
	}
}
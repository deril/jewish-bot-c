using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    class WeekDay : IAction
    {
		TelegramBotClient Bot { get; }
		const string TimeZoneId = "Europe/Kiev";

		public WeekDay(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async Task HandleAsync(long chatId)
        {
            var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
            await Bot.SendTextMessageAsync(chatId, $"Today is {currentTime.DayOfWeek}");
        }
    }
}

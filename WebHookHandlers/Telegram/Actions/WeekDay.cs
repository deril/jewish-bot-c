using System;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    public class WeekDay : IAction
    {
        private TelegramBotClient Bot { get; }
        private const string TimeZoneId = "Europe/Kiev";

        public WeekDay(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async void HandleAsync(long chatId)
        {
            var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
            await Bot.SendTextMessageAsync(chatId, $"Today is {currentTime.DayOfWeek}");
        }
    }
}

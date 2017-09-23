using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    class WeekDay : IAction
    {
        const string TimeZoneId = "Europe/Kiev";
        TelegramBotClient Bot { get; }
        long ChatId { get; }

        public WeekDay(TelegramBotClient bot, long chatId)
        {
            Bot = bot;
            ChatId = chatId;
        }

        public async Task HandleAsync()
        {
            var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
            await Bot.SendTextMessageAsync(ChatId, $"Today is {currentTime.DayOfWeek}");
        }
    }
}

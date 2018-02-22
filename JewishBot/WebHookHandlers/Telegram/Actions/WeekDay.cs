namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System;
    using System.Threading.Tasks;
    using global::Telegram.Bot;

    internal class WeekDay : IAction
    {
        private const string TimeZoneId = "Europe/Kiev";
        private readonly TelegramBotClient bot;
        private readonly long chatId;

        public WeekDay(TelegramBotClient bot, long chatId)
        {
            this.bot = bot;
            this.chatId = chatId;
        }

        public async Task HandleAsync()
        {
            var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
            await this.bot.SendTextMessageAsync(this.chatId, $"Today is {currentTime.DayOfWeek}");
        }
    }
}

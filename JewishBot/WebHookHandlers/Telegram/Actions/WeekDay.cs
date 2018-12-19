namespace JewishBot.WebHookHandlers.Telegram.Actions
{
    using System;
    using System.Threading.Tasks;

    internal class WeekDay : IAction
    {
        private const string TimeZoneId = "Europe/Kiev";
        private readonly IBotService botService;
        private readonly long chatId;

        public WeekDay(IBotService botService, long chatId)
        {
            this.botService = botService;
            this.chatId = chatId;
        }

        public async Task HandleAsync()
        {
            var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
            await this.botService.Client.SendTextMessageAsync(this.chatId, $"Today is {currentTime.DayOfWeek}");
        }
    }
}

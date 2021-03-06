﻿namespace JewishBot.Actions
{
    using System;
    using System.Threading.Tasks;
    using WebHookHandlers.Telegram;

    internal class WeekDay : IAction
    {
        private const string TimeZoneId = "Europe/Kiev";
        private readonly IBotService _botService;
        private readonly long _chatId;

        public WeekDay(IBotService botService, long chatId)
        {
            _botService = botService;
            _chatId = chatId;
        }

        public async Task HandleAsync()
        {
            var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
            await _botService.Client.SendTextMessageAsync(_chatId, $"Today is {currentTime.DayOfWeek}");
        }
    }
}
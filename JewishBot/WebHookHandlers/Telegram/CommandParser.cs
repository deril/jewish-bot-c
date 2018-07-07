namespace JewishBot.WebHookHandlers.Telegram
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class CommandParser
    {
        private const char MainDetermiter = ' ';
        private const char BotNameDelimiter = '@';

        public CommandParser(string msg)
        {
            this.Message = msg;
        }

        private string Message { get; }

        public Command Parse()
        {
            var firstSpace = this.Message.IndexOf(MainDetermiter, StringComparison.OrdinalIgnoreCase);
            string name;
            var args = new ReadOnlyCollection<string>(new List<string>());

            if (firstSpace == -1)
            {
                name = this.Message.Substring(1);
            }
            else
            {
                name = this.Message.Substring(1, firstSpace - 1);
                args = Array.AsReadOnly(this.Message.Substring(firstSpace + 1).Split());
            }

            var botNameDelimiterIndex = name.IndexOf(BotNameDelimiter, StringComparison.OrdinalIgnoreCase);

            if (botNameDelimiterIndex != -1)
            {
                name = name.Substring(0, botNameDelimiterIndex);
            }

            return new Command(name, args);
        }
    }
}
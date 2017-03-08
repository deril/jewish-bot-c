namespace JewishBot.WebHookHandlers.Telegram
{
    public class CommandParser
    {
		string Message { get; }

		public CommandParser(string msg)
        {
            Message = msg;
        }

        public char MainDetermiter { get; set; } = ' ';

        public char BotNameDelimiter { get; set; } = '@';

        public Command Parse()
        {
            var firstSpace = Message.IndexOf(MainDetermiter);
            var command = new Command();

            if (firstSpace == -1)
            {
                command.Name = Message.Substring(1);
            }
            else
            {
                command.Name = Message.Substring(1, firstSpace - 1);
                command.Arguments = Message.Substring(firstSpace + 1).Split();
            }

            var botNameDelimiterIndex = command.Name.IndexOf(BotNameDelimiter);

            if (botNameDelimiterIndex != -1)
            {
                command.Name = command.Name.Substring(0, botNameDelimiterIndex);
            }

            return command;
        }
    }
}
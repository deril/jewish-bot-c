namespace JewishBot.WebHookHandlers.Telegram
{
    public class CommandParser
    {
        public CommandParser(string msg)
        {
            this.Message = msg;
        }

        public char MainDetermiter { get; set; } = ' ';

        public char BotNameDelimiter { get; set; } = '@';

        private string Message { get; }

        public Command Parse()
        {
            var firstSpace = this.Message.IndexOf(this.MainDetermiter);
            var command = new Command();

            if (firstSpace == -1)
            {
                command.Name = this.Message.Substring(1);
            }
            else
            {
                command.Name = this.Message.Substring(1, firstSpace - 1);
                command.Arguments = this.Message.Substring(firstSpace + 1).Split();
            }

            var botNameDelimiterIndex = command.Name.IndexOf(this.BotNameDelimiter);

            if (botNameDelimiterIndex != -1)
            {
                command.Name = command.Name.Substring(0, botNameDelimiterIndex);
            }

            return command;
        }
    }
}
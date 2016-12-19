namespace JewishBot {

    public class CommandParser {
        private string message;

        public CommandParser(string msg) {
            message = msg;
        }

        public char MainDetermiter { get; set; } = ' ';

        public char BotNameDelimiter { get; set; } = '@';

        public Command Parse() {
            var firstSpace = message.IndexOf(MainDetermiter);
            var command = new Command();

            if (firstSpace == -1) {
                command.Name = message.Substring(1);
            } else {
                command.Name = message.Substring(1, firstSpace - 1);
                command.Arguments = message.Substring(firstSpace + 1).Split();
            }

            var botNameDelimiterIndex = command.Name.IndexOf(BotNameDelimiter);

            if (botNameDelimiterIndex != -1) {
                command.Name = command.Name.Substring(0, botNameDelimiterIndex);
            }

            return command;
        }
    }
}
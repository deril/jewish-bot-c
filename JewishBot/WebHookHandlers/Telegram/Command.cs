namespace JewishBot.WebHookHandlers.Telegram
{
    using System.Collections.ObjectModel;

    public class Command
    {
        public Command(string name, ReadOnlyCollection<string> arguments)
        {
            (Name, Arguments) = (name, arguments);
        }

        public string Name { get; }

        public ReadOnlyCollection<string> Arguments { get; }
    }
}
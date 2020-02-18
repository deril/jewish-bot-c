namespace JewishBot.WebHookHandlers.Telegram
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

    public class Command
    {
        public Command(string name, ReadOnlyCollection<string> arguments)
        {
            (this.Name, this.Arguments) = (name, arguments);
        }

        public string Name { get; }

        public ReadOnlyCollection<string> Arguments { get; }
    }
}
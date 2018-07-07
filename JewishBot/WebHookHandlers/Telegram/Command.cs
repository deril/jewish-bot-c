namespace JewishBot.WebHookHandlers.Telegram
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

    public class Command
    {
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Deconstruction.")]
        public Command(string name, ReadOnlyCollection<string> arguments)
        {
            (this.Name, this.Arguments) = (name, arguments);
        }

        public string Name { get; }

        public ReadOnlyCollection<string> Arguments { get; }
    }
}
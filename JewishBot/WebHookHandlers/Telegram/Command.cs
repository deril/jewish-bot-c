﻿using System.Collections.ObjectModel;

namespace JewishBot.WebHookHandlers.Telegram;

public class Command
{
    public Command(string name, ReadOnlyCollection<string> arguments)
    {
        (Name, Arguments) = (name, arguments);
    }

    public string Name { get; }

    public ReadOnlyCollection<string> Arguments { get; }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JewishBot.WebHookHandlers.Telegram;

public class CommandParser
{
    private const char MainDelimiter = ' ';
    private const char BotNameDelimiter = '@';

    public CommandParser(string msg)
    {
        Message = msg;
    }

    private string Message { get; }

    public Command Parse()
    {
        var firstSpace = Message.IndexOf(MainDelimiter, StringComparison.OrdinalIgnoreCase);
        string name;
        var args = new ReadOnlyCollection<string>(new List<string>());

        if (firstSpace == -1)
        {
            name = Message[1..];
        }
        else
        {
            name = Message[1..firstSpace];
            args = Array.AsReadOnly(Message[(firstSpace + 1)..].Split());
        }

        var botNameDelimiterIndex = name.IndexOf(BotNameDelimiter, StringComparison.OrdinalIgnoreCase);

        if (botNameDelimiterIndex != -1) name = name[..botNameDelimiterIndex];

        return new Command(name, args);
    }
}
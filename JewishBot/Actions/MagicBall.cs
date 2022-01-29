using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JewishBot.WebHookHandlers.Telegram;

namespace JewishBot.Actions;

internal class MagicBall : IAction
{
    private const string Description = @"Predicts a future.
            Usage: /ball <question>";

    private readonly List<string> _answers = new()
    {
        "It is certain",
        "It is decidedly so",
        "Without a doubt",
        "Yes definitely",
        "You may rely on it",
        "As I see it, yes",
        "Most likely",
        "Outlook good",
        "Yes",
        "Signs point to yes",
        "Don't count on it",
        "My reply is no",
        "My sources say no",
        "Outlook not so good",
        "Very doubtful"
    };

    private readonly IReadOnlyCollection<string> _args;

    private readonly List<string> _askAgainAnswers = new()
    {
        "Reply hazy try again",
        "Ask again later",
        "Better not tell you now",
        "Cannot predict now",
        "Concentrate and ask again"
    };

    private readonly IBotService _botService;
    private readonly long _chatId;
    private readonly Random _rnd = new();

    public MagicBall(IBotService botService, long chatId, IReadOnlyCollection<string> args)
    {
        _botService = botService;
        _chatId = chatId;
        _args = args;
    }

    public async Task HandleAsync()
    {
        if (_args == null)
        {
            await _botService.Client.SendTextMessageAsync(_chatId, Description).ConfigureAwait(false);
            return;
        }

        if (_rnd.Next(1, 6) == 1)
        {
            var index = _rnd.Next(_askAgainAnswers.Count + 1);
            await _botService.Client.SendTextMessageAsync(_chatId, _askAgainAnswers[index])
                .ConfigureAwait(false);
        }
        else
        {
            using var algorithm = SHA256.Create();
            var time = DateTime.Now.Ticks;
            var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(string.Join(" ", _args) + time));
            var index = (int) (ConvertHash(hash) % _answers.Count);
            await _botService.Client.SendTextMessageAsync(_chatId, _answers[index])
                .ConfigureAwait(false);
        }
    }

    private static BigInteger ConvertHash(byte[] hash)
    {
        // make int unsigned
        var uhash = new byte[hash.Length + 1];
        hash.CopyTo(uhash, 0);
        uhash[hash.Length] = 00;
        return new BigInteger(uhash);
    }
}
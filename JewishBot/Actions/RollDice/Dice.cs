using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JewishBot.Actions.RollDice;

public class Dice
{
    private const string CommonRollRegexPattern = "d *\\d+(?: *k *\\d+)?";
    private const string StrictRollPattern = "(?:(?:\\d* +)|(?:\\d+ *)|^)" + CommonRollRegexPattern;
    private readonly int _die;
    private readonly int _quantity;

    private readonly Random _rnd = new();

    public Dice(string toParse)
    {
        var sections = toParse.Split('d');
        if (!string.IsNullOrEmpty(sections[0]))
            if (!int.TryParse(sections[1], out _die))
                _die = 6;

        _quantity = 1;
        if (string.IsNullOrEmpty(sections[0])) return;

        if (!int.TryParse(sections[0], out _quantity)) _quantity = 1;
    }

    public static bool CanParse(string toParse)
    {
        if (string.IsNullOrWhiteSpace(toParse)) return false;

        var trimmedSource = toParse.Trim();
        var strictRollRegex = new Regex(StrictRollPattern);
        var rollMatch = strictRollRegex.Match(trimmedSource);

        return rollMatch.Success && rollMatch.Value == trimmedSource;
    }

    public int GetSum()
    {
        var rolls = GetRolls();
        return rolls.Sum();
    }

    private int Roll()
    {
        return _rnd.Next(_die) + 1;
    }

    private IEnumerable<int> GetRolls()
    {
        var rolls = new List<int>(_quantity);

        for (var i = 0; i < _quantity; i++) rolls.Add(Roll());

        return rolls;
    }
}
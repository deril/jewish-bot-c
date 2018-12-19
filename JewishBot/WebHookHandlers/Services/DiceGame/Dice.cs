namespace JewishBot.WebHookHandlers.Services.DiceGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Dice
    {
        private const string CommonRollRegexPattern = "d *\\d+(?: *k *\\d+)?";
        private const string StrictRollPattern = "(?:(?:\\d* +)|(?:\\d+ *)|^)" + CommonRollRegexPattern;

        private readonly Random rnd = new Random();
        private readonly int quantity;
        private readonly int die;

        public Dice(string toParse)
        {
            var sections = toParse.Split('d');
            if (!string.IsNullOrEmpty(sections[0]))
            {
                if (!int.TryParse(sections[1], out this.die))
                {
                    this.die = 6;
                }
            }

            this.quantity = 1;
            if (string.IsNullOrEmpty(sections[0]))
            {
                return;
            }

            if (!int.TryParse(sections[0], out this.quantity))
            {
                this.quantity = 1;
            }
        }

        public static bool CanParse(string toParse)
        {
            if (string.IsNullOrWhiteSpace(toParse))
            {
                return false;
            }

            var trimmedSource = toParse.Trim();
            var strictRollRegex = new Regex(StrictRollPattern);
            var rollMatch = strictRollRegex.Match(trimmedSource);

            return rollMatch.Success && rollMatch.Value == trimmedSource;
        }

        public int GetSum()
        {
            var rolls = this.GetRolls();
            return rolls.Sum();
        }

        private int Roll()
        {
            return this.rnd.Next(this.die) + 1;
        }

        private IEnumerable<int> GetRolls()
        {
            var rolls = new List<int>(this.quantity);

            for (var i = 0; i < this.quantity; i++)
            {
                rolls.Add(this.Roll());
            }

            return rolls;
        }
    }
}
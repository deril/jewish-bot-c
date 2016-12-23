using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JewishBot.Services.DiceGame {
    public class Dice {
        private readonly Random rnd = new Random();
        public const string CommonRollRegexPattern = "d *\\d+(?: *k *\\d+)?";
        public const string StrictRollPattern = "(?:(?:\\d* +)|(?:\\d+ *)|^)" + CommonRollRegexPattern;

        public int Quantity { get; set; }
        public int Die { get; set; }

        public Dice(string toParse) {
            var sections = toParse.Split('d');
            Die = Convert.ToInt32(sections[1]);
            Quantity = 1;
            if (!string.IsNullOrEmpty(sections[0])) {
                Quantity = Convert.ToInt32(sections[0]);
            }
        }

        public static bool CanParse(string toParse) {
            if (string.IsNullOrWhiteSpace(toParse))
                return false;

            var trimmedSource = toParse.Trim();
            var strictRollRegex = new Regex(StrictRollPattern);
            var rollMatch = strictRollRegex.Match(trimmedSource);

            return rollMatch.Success && rollMatch.Value == trimmedSource;
        }

        private int Roll() {
            return rnd.Next(Die) + 1;
        }

        public IEnumerable<int> GetRolls() {
            List<int> rolls = new List<int>(Quantity);

            for (int i = 0; i < Quantity; i++) {
                rolls.Add(Roll());
            }

            return rolls;
        }

        public int GetSum() {
            var rolls = GetRolls();
            return rolls.Sum();
        }
    }
}
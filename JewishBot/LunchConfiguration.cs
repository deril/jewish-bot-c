namespace JewishBot
{
    using System.Collections.Generic;

    public class LunchConfiguration
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public List<string> Members { get; set; }

        public Dictionary<string, string> Providers { get; set; }
    }
}
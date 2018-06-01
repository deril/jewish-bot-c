namespace JewishBot.WebHookHandlers.Telegram.Services.Lunch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Web;
    using CsQuery;

    public class LunchApi
    {
        private const string ObediUrl = "http://lunches.vps.lviv.ua/staff/1/bills";
        private const string KoloSmakuUrl = "http://lunches.vps.lviv.ua/staff/5/bills";
        private readonly HttpClient httpClient = new HttpClient();
        private readonly FormUrlEncodedContent authParams;
        private readonly string[] members;

        public LunchApi(string email, string password, string[] members)
        {
            this.authParams = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("auth[email]", email),
                                new KeyValuePair<string, string>("auth[password]", password)
                            });
            this.members = members;
        }

        public string Invoke()
        {
            var obedi = this.RequestResult(ObediUrl);
            var koloSmaku = this.RequestResult(KoloSmakuUrl);
            var obediResults = this.ParseMeals(obedi);
            var koloSmakuResults = this.ParseMeals(koloSmaku);
            var obediFormated = this.FormatMeals(obediResults);
            var koloSmakuFormated = this.FormatMeals(koloSmakuResults);
            var output = new StringBuilder();
            if (!string.IsNullOrEmpty(obediFormated))
            {
                output.Append($"Obedi:\n\n{obediFormated}\n\n");
            }

            if (!string.IsNullOrEmpty(koloSmakuFormated))
            {
                output.Append($"Kolo Smaku:\n\n{koloSmakuFormated}\n\n");
            }

            return string.IsNullOrEmpty(output.ToString()) ? "Not Found." : output.ToString();
        }

        private IEnumerable<Order> ParseMeals(CQ provider)
        {
            return provider["strong.mr_r_1"].Select(element =>
            {
                var names = element.NextSibling.NodeValue.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                return names.Select(name => new Order
                {
                    Meal = HttpUtility.HtmlDecode(element.InnerText)?.Replace(":", string.Empty),
                    Name = name.Trim()
                });
            }).SelectMany(i => i);
        }

        private CQ RequestResult(string url)
        {
            var requestResponce = this.httpClient.PostAsync(url, this.authParams).Result;
            requestResponce.EnsureSuccessStatusCode();
            var html = requestResponce.Content.ReadAsStringAsync().Result;
            var dom = CQ.Create(html);
            return dom["div#print"];
        }

        private string FormatMeals(IEnumerable<Order> orders)
        {
            var i = orders.Where(order => Array.Exists(this.members, member => order.Name.ToLowerInvariant().Contains(member.ToLowerInvariant())))
                          .ToLookup(order => order.Name, order => order.Meal)
                          .Select(group => $"☻ {group.Key}\n\n{string.Join("\n", group.Select(meal => $"• {meal}"))}");
            return string.Join("\n\n", i);
        }

        private struct Order
        {
            public string Meal { get; set; }

            public string Name { get; set; }
        }
    }
}

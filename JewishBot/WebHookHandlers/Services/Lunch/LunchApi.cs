namespace JewishBot.WebHookHandlers.Services.Lunch
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
        private readonly Uri obediUrl = new Uri("http://lunches.vps.lviv.ua/staff/1/bills");
        private readonly Uri koloSmakuUrl = new Uri("http://lunches.vps.lviv.ua/staff/5/bills");
        private readonly string email;
        private readonly string password;
        private readonly string[] members;
        private readonly IHttpClientFactory clientFactory;

        public LunchApi(string email, string password, string[] members, IHttpClientFactory clientFactory)
        {
            this.email = email;
            this.password = password;
            this.members = members;
            this.clientFactory = clientFactory;
        }

        public string Invoke()
        {
            var obedi = this.RequestResult(this.obediUrl);
            var koloSmaku = this.RequestResult(this.koloSmakuUrl);
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
                    Meal = HttpUtility.HtmlDecode(element.InnerText)?.Replace(":", string.Empty, StringComparison.OrdinalIgnoreCase),
                    Name = name.Trim()
                });
            }).SelectMany(i => i);
        }

        private CQ RequestResult(Uri url)
        {
            var client = this.clientFactory.CreateClient();
            string html;
            using (var authParams = new FormUrlEncodedContent(new[]
                {
                                new KeyValuePair<string, string>("auth[email]", this.email),
                                new KeyValuePair<string, string>("auth[password]", this.password)
                            }))
            {
                var requestResponce = client.PostAsync(url, authParams).Result;
                requestResponce.EnsureSuccessStatusCode();
                html = requestResponce.Content.ReadAsStringAsync().Result;
            }

            var dom = CQ.Create(html);
            return dom["div#print"];
        }

        private string FormatMeals(IEnumerable<Order> orders)
        {
            var i = orders.Where(order => Array.Exists(this.members, member => order.Name.ToUpperInvariant().Contains(member.ToUpperInvariant(), StringComparison.CurrentCulture)))
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

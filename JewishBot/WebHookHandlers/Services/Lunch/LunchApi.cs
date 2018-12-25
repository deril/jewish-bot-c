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
        private readonly string email;
        private readonly string password;
        private readonly List<string> members;
        private readonly IHttpClientFactory clientFactory;
        private readonly Dictionary<string, string> providers;

        public LunchApi(string email, string password, List<string> members, Dictionary<string, string> providers, IHttpClientFactory clientFactory)
        {
            this.email = email;
            this.password = password;
            this.members = members;
            this.clientFactory = clientFactory;
            this.providers = providers;
        }

        public string Invoke()
        {
            var output = new StringBuilder();
            foreach (var (providerName, providerUrl) in this.providers)
            {
                var formatted = this.FormatProvider(providerUrl);
                if (!string.IsNullOrEmpty(formatted))
                {
                    output.Append($"{providerName}:\n\n{formatted}\n\n");
                }
            }

            return string.IsNullOrEmpty(output.ToString()) ? "Not Found." : output.ToString();
        }

        private string FormatProvider(string url)
        {
            var response = this.RequestResult(new Uri(url));
            var parsed = this.ParseMeals(response);
            return this.FormatMeals(parsed);
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
                var requestResponse = client.PostAsync(url, authParams).Result;
                requestResponse.EnsureSuccessStatusCode();
                html = requestResponse.Content.ReadAsStringAsync().Result;
            }

            var dom = CQ.Create(html);
            return dom["div#print"];
        }

        private string FormatMeals(IEnumerable<Order> orders)
        {
            var i = orders.Where(order => this.members.Exists(member => order.Name.ToUpperInvariant().Contains(member.ToUpperInvariant(), StringComparison.CurrentCulture)))
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

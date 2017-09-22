using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using CsQuery;

namespace JewishBot.WebHookHandlers.Telegram.Services.Lunch
{
    public class LunchApi
    {
        const string BaseUrl = "http://lunches.vps.lviv.ua/staff";
        const string ObediUrl = "http://lunches.vps.lviv.ua/staff/1/bills";
        const string KoloSmakuUrl = "http://lunches.vps.lviv.ua/staff/5/bills";
        readonly HttpClient _httpClient = new HttpClient();
        readonly FormUrlEncodedContent _authParams;
        readonly string[] _members;

		struct Order
		{
			public string Meal { get; set; }
			public String Name { get; set; }
		}

        public LunchApi(string email, string password, string members)
        {
            _authParams = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("auth[email]", email),
                                new KeyValuePair<string, string>("auth[password]", password)
                            });
            _members = members.Split(",");
        }

        public string Invoke()
        {
            var obedi = RequestResult(ObediUrl);
            var koloSmaku = RequestResult(KoloSmakuUrl);
            var obediResults = ParseMeals(obedi);
            var koloSmakuResults = ParseMeals(koloSmaku);
            var obediFormated = FormatMeals(obediResults);
            var koloSmakuFormated = FormatMeals(koloSmakuResults);
            var output = new StringBuilder();
            if (!String.IsNullOrEmpty(obediFormated))
                output.Append($"Obedi:\n\n{obediFormated}\n\n");
			if (!String.IsNullOrEmpty(koloSmakuFormated))
				output.Append($"Kolo Smaku:\n\n{koloSmakuFormated}\n\n");
            return String.IsNullOrEmpty(output.ToString()) ? "Not Found." : output.ToString();
        }

		IEnumerable<Order> ParseMeals(CQ provider)
		{
			return provider["strong.mr_r_1"].Select(element =>
			{
				var names = element.NextSibling.NodeValue.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
				return names.Select(name => new Order
				{
					Meal = HttpUtility.HtmlDecode(element.InnerText).Replace(":", string.Empty),
					Name = name.Trim()
				});
			}).SelectMany(i => i);
		}

        CQ RequestResult(string url)
        {
            var requestResponce = _httpClient.PostAsync(url, _authParams).Result;
            requestResponce.EnsureSuccessStatusCode();
            var html = requestResponce.Content.ReadAsStringAsync().Result;
            var dom = CQ.Create(html);
            return dom["div#print"];
        }

		string FormatMeals(IEnumerable<Order> orders)
		{
			var i = orders.Where(order => Array.Exists(_members, member => order.Name.ToLowerInvariant().Contains(member.ToLowerInvariant()))).
			ToLookup(order => order.Name, order => order.Meal).ToList().
			Select(group => $"☻ {group.Key}\n\n{String.Join("\n", group.Select(meal => $"• {meal}"))}");
			return String.Join("\n\n", i);
		}
    }
}

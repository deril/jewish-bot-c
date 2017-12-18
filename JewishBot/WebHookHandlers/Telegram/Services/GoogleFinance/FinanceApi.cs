using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.GoogleFinance
{
    public class FinanceApi : ApiService
    {
        const string BaseUrl = "https://finance.google.com/finance/converter";

        public override string BuildEndpointRoute(string[] arguments)
        {
            var parameters = new Dictionary<string, string>
            {
                {"a", arguments[0]},
                {"from", arguments[1]},
                {"to", arguments[2]}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }

        public override async Task<string> InvokeAsync(string[] arguments)
        {
            string rate;

            try
            {
                await Requester(arguments);
                var separator = new string[] { "<span class=bld>" };
                var split = Content.Split(separator, StringSplitOptions.None);
                rate = split[1].Split(' ')[0];
            }
            catch
            {
                rate = "0.00";
            }

            return rate;
        }
    }
}

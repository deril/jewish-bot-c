namespace JewishBot.WebHookHandlers.Telegram.Services.GoogleFinance
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.WebUtilities;

    public class FinanceApi : ApiService
    {
        private const string BaseUrl = "https://finance.google.com/finance/converter";

        public override string BuildEndpointRoute(string[] arguments)
        {
            var parameters = new Dictionary<string, string>
            {
                { "a", arguments[0] },
                { "from", arguments[1] },
                { "to", arguments[2] }
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }

        public override async Task<string> InvokeAsync(string[] arguments)
        {
            string rate;

            try
            {
                await this.Requester(arguments);
                var separator = new string[] { "<span class=bld>" };
                var split = this.Content.Split(separator, StringSplitOptions.None);
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

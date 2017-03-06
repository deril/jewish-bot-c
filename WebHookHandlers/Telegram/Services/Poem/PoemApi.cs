using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace JewishBot.WebHookHandlers.Telegram.Services.Poem
{
    public class PoemApi : ApiService
    {
        private const string BaseUrl = "https://poem.alv.in/api/generate";

        public override string BuildEndpointRoute(string term)
        {
            return BaseUrl;
        }

        public async void Like(string hashid)
        {
            try
            {
                QueryUrl = BuildEndpointLikeRoute(hashid);
                Response = await HttpClient.GetAsync(QueryUrl);
                Response.EnsureSuccessStatusCode();
                Content = await Response.Content.ReadAsStringAsync();

                this.QueryUrl = BaseUrl;
            }
            catch
            {
                // TODO: implement here logging
            }
        }

        private static string BuildEndpointLikeRoute(string hashid)
        {
            var parameters = new Dictionary<string, string>
            {
                {"hashid", hashid}
            };

            return QueryHelpers.AddQueryString(BaseUrl, parameters);
        }
    }
}
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.WebUtilities;

//namespace JewishBot.WebHookHandlers.Telegram.Services.Poem
//{
//    public class PoemApi : ApiServiceJson
//    {
//        const string BaseUrl = "https://poem.alv.in/api/generate";

//        public override string BuildEndpointRoute(string[] arguments) => BaseUrl;

//        public async Task Like(string hashid)
//        {
//            try
//            {
//                QueryUrl = BuildEndpointLikeRoute(hashid);
//                Response = await HttpClient.GetAsync(QueryUrl);
//                Response.EnsureSuccessStatusCode();
//                Content = await Response.Content.ReadAsStringAsync();

//                QueryUrl = BaseUrl;
//            }
//            catch
//            {
//                // TODO: implement here logging
//            }
//        }

//        static string BuildEndpointLikeRoute(string hashid)
//        {
//            var parameters = new Dictionary<string, string>
//            {
//                {"hashid", hashid}
//            };

//            return QueryHelpers.AddQueryString(BaseUrl, parameters);
//        }
//    }
//}
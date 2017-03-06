using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JewishBot.WebHookHandlers.Telegram.Services
{
    public abstract class ApiService
    {
        public string QueryUrl;
        public string Content = string.Empty;
        public HttpResponseMessage Response;
        public HttpClient HttpClient = new HttpClient();

        public async Task<T> Invoke<T>(string pair) where T : new()
        {
            try
            {
                QueryUrl = BuildEndpointRoute(pair);
                Response = await HttpClient.GetAsync(QueryUrl);
                Response.EnsureSuccessStatusCode();
                Content = await Response.Content.ReadAsStringAsync();
            }
            catch
            {
                // TODO: implement here logging
                // Create nullable JSON
                Content = JsonConvert.SerializeObject(new T());
            }

            return JsonConvert.DeserializeObject<T>(Content);
        }

        public abstract string BuildEndpointRoute(string argument);
    }
}
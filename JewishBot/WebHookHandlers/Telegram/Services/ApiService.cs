using System.Net.Http;
using System.Threading.Tasks;

namespace JewishBot.WebHookHandlers.Telegram.Services
{
    public abstract class ApiService
    {
        public string QueryUrl;
        public string Content = string.Empty;
        public HttpResponseMessage Response;
        public HttpClient HttpClient = new HttpClient();

        public abstract string BuildEndpointRoute(string[] arguments);
        public abstract Task<string> InvokeAsync(string[] arguments);

        protected virtual async Task Requester(string[] pair)
        {
            QueryUrl = BuildEndpointRoute(pair);
            Response = await HttpClient.GetAsync(QueryUrl);
            Response.EnsureSuccessStatusCode();
            Content = await Response.Content.ReadAsStringAsync();
        }
    }
}

namespace JewishBot.WebHookHandlers.Telegram.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public abstract class ApiService
    {
        private readonly HttpClient httpClient = new HttpClient();

        protected string Content { get; set; } = string.Empty;

        public abstract Task<string> InvokeAsync(string[] arguments);

        protected abstract string BuildEndpointRoute(string[] arguments);

        protected async Task Requester(string[] pair)
        {
            var queryUrl = this.BuildEndpointRoute(pair);
            var response = await this.httpClient.GetAsync(queryUrl);
            response.EnsureSuccessStatusCode();
            this.Content = await response.Content.ReadAsStringAsync();
        }
    }
}

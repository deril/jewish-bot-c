namespace JewishBot.WebHookHandlers.Telegram.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public abstract class ApiService
    {
        private readonly HttpClient httpClient = new HttpClient();
        private string queryUrl;
        private HttpResponseMessage response;

        public string Content { get; set; } = string.Empty;

        public abstract string BuildEndpointRoute(string[] arguments);

        public abstract Task<string> InvokeAsync(string[] arguments);

        protected virtual async Task Requester(string[] pair)
        {
            this.queryUrl = this.BuildEndpointRoute(pair);
            this.response = await this.httpClient.GetAsync(this.queryUrl);
            this.response.EnsureSuccessStatusCode();
            this.Content = await this.response.Content.ReadAsStringAsync();
        }
    }
}

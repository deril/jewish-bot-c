namespace JewishBot.WebHookHandlers.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public abstract class ApiService : IDisposable
    {
        private readonly HttpClient httpClient = new HttpClient();
        private bool disposed = false;

        ~ApiService()
        {
            this.Dispose(false);
        }

        protected string Content { get; set; } = string.Empty;

        public abstract Task<string> InvokeAsync(string[] arguments);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.httpClient.Dispose();
            }

            this.disposed = true;
        }

        protected abstract Uri BuildEndpointRoute(string[] arguments);

        protected async Task Requester(string[] pair)
        {
            var queryUrl = this.BuildEndpointRoute(pair);
            var response = await this.httpClient.GetAsync(queryUrl);
            response.EnsureSuccessStatusCode();
            this.Content = await response.Content.ReadAsStringAsync();
        }
    }
}

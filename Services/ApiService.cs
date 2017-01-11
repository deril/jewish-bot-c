using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JewishBot.Services {
    public abstract class ApiService {
        private string baseUrl;
        public string queryUrl;
        public string content = string.Empty;
        public HttpResponseMessage response;
        public HttpClient httpClient = new HttpClient();
        
        public async Task<T> Invoke<T>(string pair) where T : new() {
            try {
                this.queryUrl = buildEndpointRoute(pair);
                response = await httpClient.GetAsync(this.queryUrl);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();

                this.queryUrl = this.baseUrl;
            } catch {
                // TODO: implement here logging
                // Create nullable JSON
                content = JsonConvert.SerializeObject(new T());
            }

            return JsonConvert.DeserializeObject<T>(content);
        }

        public abstract string buildEndpointRoute(string argument);
    }
}
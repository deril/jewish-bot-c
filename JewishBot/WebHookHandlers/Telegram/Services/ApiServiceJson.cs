namespace JewishBot.WebHookHandlers.Telegram.Services
{
    using System;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public abstract class ApiServiceJson : ApiService
    {
        public async Task<T> InvokeAsync<T>(string[] arguments)
            where T : new()
        {
            try
            {
                await this.Requester(arguments);
            }
            catch
            {
                // Create nullable JSON
                this.Content = JsonConvert.SerializeObject(new T());
            }

            return JsonConvert.DeserializeObject<T>(this.Content);
        }

        public override Task<string> InvokeAsync(string[] arguments) => throw new NotImplementedException();
    }
}

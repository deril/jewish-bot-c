using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JewishBot.WebHookHandlers.Telegram.Services
{
    public abstract class ApiServiceJson : ApiService
    {
        public async Task<T> InvokeAsync<T>(string[] arguments) where T : new()
        {
            try
            {
                await Requester(arguments);
            }
            catch
            {
                // TODO: implement here logging
                // Create nullable JSON
                Content = JsonConvert.SerializeObject(new T());
            }

            return JsonConvert.DeserializeObject<T>(Content);
        }

        public override Task<string> InvokeAsync(string[] arguments) => throw new NotImplementedException();
    }
}
